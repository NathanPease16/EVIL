using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

public enum RateMode
{
    time,
    speed,
}

public class WiggleWarp : MonoBehaviour
{
    #region Keyframes
    private static Keyframe[] linearKeys = new Keyframe[] { new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1) };
    private static Keyframe[] quadraticKeys = new Keyframe[] { new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 2, 0) };
    private static Keyframe[] cubicKeys = new Keyframe[] { new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 3, 0) };
    private static Keyframe[] bouncingKeys = new Keyframe[] { new Keyframe(0, 0, 0, -1.5f), new Keyframe(1, 1, -1.5f, 0) };
    private static Keyframe[] overshootKeys = new Keyframe[] { new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, -1.5f, 0) };
    private static Keyframe[] recoveryKeys = new Keyframe[] { new Keyframe(0, 0, 0, -1.5f), new Keyframe(1, 1, 0, 0) };
    private static Keyframe[] easeInKeys = new Keyframe[] { new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 1, 0, .25f, 0) };
    private static Keyframe[] easeOutKeys = new Keyframe[] { new Keyframe(0, 0, 0, 1, 0, .25f), new Keyframe(1, 1, 0, 0) };
    private static Keyframe[] easeInOutKeys = new Keyframe[] { new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0) };
    #endregion

    #region Curves
    public static readonly AnimationCurve linear = new AnimationCurve(linearKeys);
    public static readonly AnimationCurve quadratic = new AnimationCurve(quadraticKeys);
    public static readonly AnimationCurve cubic = new AnimationCurve(cubicKeys);
    public static readonly AnimationCurve bouncing = new AnimationCurve(bouncingKeys);
    public static readonly AnimationCurve overshoot = new AnimationCurve(overshootKeys);
    public static readonly AnimationCurve recovery = new AnimationCurve(recoveryKeys);
    public static readonly AnimationCurve easeIn = new AnimationCurve(easeInKeys);
    public static readonly AnimationCurve easeOut = new AnimationCurve(easeOutKeys);
    public static readonly AnimationCurve easeInOut = new AnimationCurve(easeInOutKeys);
    #endregion

    private static WiggleWarp instance;
    private static Dictionary<int, Coroutine> routines;
    private static HashSet<int> pausedRoutines;
    private static int currentID;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        routines = new Dictionary<int, Coroutine>();
        pausedRoutines = new HashSet<int>();
    }

    #region Float
    public static int InterpolateFloat<T>(T target, string property, float goal, float rate, 
                                          AnimationCurve curve=null, RateMode mode=RateMode.time) where T : class
    {
        PropertyInfo propertyInfo = GetInterpolatablePropertyInfo(typeof(T), property, typeof(float), target);
        float start = (float)propertyInfo.GetValue(target);

        return InterpolateFloat(target, property, start, goal, rate, curve, mode);
    }

    public static int InterpolateFloat<T>(T target, string property, float start, float goal, float rate, 
                                          AnimationCurve curve=null, RateMode mode=RateMode.time) where T : class
    {
        PropertyInfo propertyInfo = GetInterpolatablePropertyInfo(typeof(T), property, typeof(float), target);

        curve = ValidateCurve(curve);
        
        if (mode == RateMode.speed)
            rate = Mathf.Abs(goal - start) / rate;

        Coroutine routine = instance.StartCoroutine(instance.InterpolateFloatAsync(target, propertyInfo, start, goal, 
                                                                                   rate, curve, currentID));
        AddRoutine(routine);

        return currentID - 1;
    }
    #endregion

    #region Vector2
    public static int InterpolateVector2<T>(T target, string property, Vector2 goal, float rate, 
                                            AnimationCurve curve=null, RateMode mode=RateMode.time) where T : class
    {
        PropertyInfo propertyInfo = GetInterpolatablePropertyInfo(typeof(T), property, typeof(Vector2), target);
        Vector2 start = (Vector2)propertyInfo.GetValue(target);

        return InterpolateVector2(target, property, start, goal, rate, curve, mode);
    }

    public static int InterpolateVector2<T>(T target, string property, Vector2 start, Vector2 goal, float rate, 
                                            AnimationCurve curve=null, RateMode mode=RateMode.time) where T : class
    {
        PropertyInfo propertyInfo = GetInterpolatablePropertyInfo(typeof(T), property, typeof(Vector2), target);
        
        ValidateCurve(curve);

        if (mode == RateMode.speed)
            rate = Vector2.Distance(start, goal) / rate;
        
        Coroutine routine = instance.StartCoroutine(instance.InterpolateVector2Async(target, propertyInfo, start, goal, 
                                                                                     rate, curve, currentID));
        AddRoutine(routine);

        return currentID - 1;
    }
    #endregion

    #region Vector3
    public static int InterpolateVector3<T>(T target, string property, Vector3 goal, float rate, 
                                            AnimationCurve curve=null, RateMode mode=RateMode.time) where T : class
    {
        PropertyInfo propertyInfo = GetInterpolatablePropertyInfo(typeof(T), property, typeof(Vector3), target);
        Vector3 start = (Vector3)propertyInfo.GetValue(target);

        return InterpolateVector3(target, property, start, goal, rate, curve, mode);
    }

    public static int InterpolateVector3<T>(T target, string property, Vector3 start, Vector3 goal, float rate, 
                                            AnimationCurve curve=null, RateMode mode=RateMode.time) where T : class
    {
        PropertyInfo propertyInfo = GetInterpolatablePropertyInfo(typeof(T), property, typeof(Vector3), target);
        
        curve = ValidateCurve(curve);

        if (mode == RateMode.speed)
            rate = Vector3.Distance(start, goal) / rate;
        
        Coroutine routine = instance.StartCoroutine(instance.InterpolateVector3Async(target, propertyInfo, start, goal, 
                                                                                     rate, curve, currentID));
        AddRoutine(routine);

        return currentID - 1;
    }
    #endregion

    #region Quaternion
    public static int InterpolateQuaternion<T>(T target, string property, Quaternion goal, float rate, 
                                               AnimationCurve curve=null, RateMode mode=RateMode.time) where T : class
    {
        PropertyInfo propertyInfo = GetInterpolatablePropertyInfo(typeof(T), property, typeof(Quaternion), target);
        Quaternion start = (Quaternion)propertyInfo.GetValue(target);

        return InterpolateQuaternion(target, property, start, goal, rate, curve, mode);
    }

    public static int InterpolateQuaternion<T>(T target, string property, Quaternion start, Quaternion goal, float rate, 
                                               AnimationCurve curve=null, RateMode mode=RateMode.time) where T : class
    {
        PropertyInfo propertyInfo = GetInterpolatablePropertyInfo(typeof(T), property, typeof(Quaternion), target);
        
        curve = ValidateCurve(curve);

        Vector3 startEuler = start.eulerAngles;
        Vector3 endEuler = goal.eulerAngles;
        if (mode == RateMode.speed)
            rate = Vector3.Distance(startEuler, endEuler) / rate;
        
        Coroutine routine = instance.StartCoroutine(instance.InterpolateQuationAsync(target, propertyInfo, start, goal, 
                                                                                     rate, curve, currentID));
        AddRoutine(routine);

        return currentID - 1;
    }
    #endregion

    #region Color
    public static int InterpolateColor<T>(T target, string property, Color goal, float rate, 
                                          AnimationCurve curve=null, RateMode mode=RateMode.time) where T : class
    {
        PropertyInfo propertyInfo = GetInterpolatablePropertyInfo(typeof(T), property, typeof(Color), target);
        Color start = (Color)propertyInfo.GetValue(target);

        return InterpolateColor(target, property, start, goal, rate, curve, mode);
    }

    public static int InterpolateColor<T>(T target, string property, Color start, Color goal, float rate, 
                                          AnimationCurve curve=null, RateMode mode=RateMode.time) where T : class
    {
        PropertyInfo propertyInfo = GetInterpolatablePropertyInfo(typeof(T), property, typeof(Color), target);
        
        curve = ValidateCurve(curve);

        Vector4 startV4 = new Vector4(start.r, start.g, start.b, start.a);
        Vector4 goalV4 = new Vector4(goal.r, goal.g, goal.b, goal.a);

        if (mode == RateMode.speed)
            rate = Vector4.Distance(startV4, goalV4) / rate;
        
        Coroutine routine = instance.StartCoroutine(instance.InterpolateColorAsync(target, propertyInfo, start, goal, 
                                                                                   rate, curve, currentID));
        AddRoutine(routine);

        return currentID - 1;
    }
    #endregion

    #region Routines
    private static void AddRoutine(Coroutine routine)
    {
        routines.Add(currentID, routine);
        currentID++;
    }

    public static bool DoesInterpolationExist(int id)
    {
        return routines.ContainsKey(id);
    }

    public static bool IsInterpolationPaused(int id)
    {
        return pausedRoutines.Contains(id);
    }

    public static void PauseInterpolation(int id)
    {
        if (routines.ContainsKey(id))
        {
            if (!pausedRoutines.Contains(id))
                pausedRoutines.Add(id);
            else
                Debug.LogWarning($"Interpolation with ID {id} is already paused!");
        }
        else
             throw new KeyNotFoundException($"Interpolation with ID {id} does not exist!");
    }

    public static void ResumeInterpolation(int id)
    {
        if (pausedRoutines.Contains(id))
            pausedRoutines.Remove(id);
        else
            throw new KeyNotFoundException($"Interpolation with ID {id} is already active or does not exist!");
    }

    public static void CancelInterpolation(int id)
    {
        if (routines.ContainsKey(id))
        {
            instance.StopCoroutine(routines[id]);
            routines.Remove(id);

            if (IsInterpolationPaused(id))
                pausedRoutines.Remove(id);
        }
        else
             throw new KeyNotFoundException($"Interpolation with ID {id} does not exist!");
    }

    private IEnumerator InterpolateFloatAsync<T>(T target, PropertyInfo property,float start, float goal, 
                                                   float time, AnimationCurve curve, int id)
    {
        Debug.Log(id);
        for (float t = 0; t <= 1; t += Time.deltaTime/time)
        {
            if (IsInterpolationPaused(id))
            {
                t -= Time.deltaTime / time;
                yield return null;
                continue;
            }

            property.SetValue(target, Mathf.LerpUnclamped(start, goal, curve.Evaluate(t)));
            yield return null;
        }
        
        property.SetValue(target, Mathf.LerpUnclamped(start, goal, curve.Evaluate(1)));

        routines.Remove(id);

        if (IsInterpolationPaused(id))
            pausedRoutines.Remove(id);
    }

    private IEnumerator InterpolateVector2Async<T>(T target, PropertyInfo property, Vector2 start, Vector2 goal, 
                                                   float time, AnimationCurve curve, int id)
    {
        for (float t= 0; t <= 1; t += Time.deltaTime/time)
        {
            if (IsInterpolationPaused(id))
            {
                t -= Time.deltaTime / time;
                yield return null;
                continue;
            }

            Vector2 vector = new Vector2(
                Mathf.LerpUnclamped(start.x, goal.x, curve.Evaluate(t)),
                Mathf.LerpUnclamped(start.y, goal.y, curve.Evaluate(t))
            );
            property.SetValue(target, vector);

            yield return null;
        }

        Vector2 end = new Vector2(
            Mathf.LerpUnclamped(start.x, goal.x, curve.Evaluate(1)),
            Mathf.LerpUnclamped(start.y, goal.y, curve.Evaluate(1))
        );

        property.SetValue(target, end);

        routines.Remove(id);

        if (IsInterpolationPaused(id))
            pausedRoutines.Remove(id);
    }

    private IEnumerator InterpolateVector3Async<T>(T target, PropertyInfo property, Vector3 start, Vector3 goal, 
                                                   float time, AnimationCurve curve, int id)
    {
        for (float t= 0; t <= 1; t += Time.deltaTime/time)
        {
            if (IsInterpolationPaused(id))
            {
                t -= Time.deltaTime;
                yield return null;
                continue;
            }

            Vector3 vector = new Vector3(
                Mathf.LerpUnclamped(start.x, goal.x, curve.Evaluate(t)),
                Mathf.LerpUnclamped(start.y, goal.y, curve.Evaluate(t)),
                Mathf.LerpUnclamped(start.z, goal.z, curve.Evaluate(t))
            );
            property.SetValue(target, vector);

            yield return null;
        }

        Vector3 end = new Vector3(
            Mathf.LerpUnclamped(start.x, goal.x, curve.Evaluate(1)),
            Mathf.LerpUnclamped(start.y, goal.y, curve.Evaluate(1)),
            Mathf.LerpUnclamped(start.z, goal.z, curve.Evaluate(1))
        );

        property.SetValue(target, end);

        routines.Remove(id);

        if (IsInterpolationPaused(id))
            pausedRoutines.Remove(id);
    }

    private IEnumerator InterpolateQuationAsync<T>(T target, PropertyInfo property, Quaternion start, Quaternion goal, 
                                                   float time, AnimationCurve curve, int id)
    {
        Vector3 startEuler = start.eulerAngles;
        Vector3 goalEuler = goal.eulerAngles;

        for (float t= 0; t <= 1; t += Time.deltaTime/time)
        {
            if (IsInterpolationPaused(id))
            {
                t -= Time.deltaTime;
                yield return null;
                continue;
            }

            Vector3 euler = new Vector3(
                Mathf.LerpUnclamped(startEuler.x, goalEuler.x, curve.Evaluate(t)),
                Mathf.LerpUnclamped(startEuler.y, goalEuler.y, curve.Evaluate(t)),
                Mathf.LerpUnclamped(startEuler.z, goalEuler.z, curve.Evaluate(t))
            );

            Quaternion quaternion = Quaternion.Euler(euler);
            property.SetValue(target, quaternion);

            yield return null;
        }

        Vector3 endEuler = new Vector3(
            Mathf.LerpUnclamped(startEuler.x, goalEuler.x, curve.Evaluate(1)),
            Mathf.LerpUnclamped(startEuler.y, goalEuler.y, curve.Evaluate(1)),
            Mathf.LerpUnclamped(startEuler.z, goalEuler.z, curve.Evaluate(1))
        );
            
        Quaternion end = Quaternion.Euler(endEuler);
        property.SetValue(target, end);

        routines.Remove(id);

        if (IsInterpolationPaused(id))
            pausedRoutines.Remove(id);
    }

        private IEnumerator InterpolateColorAsync<T>(T target, PropertyInfo property, Color start, Color goal, 
                                                   float time, AnimationCurve curve, int id)
    {
        for (float t= 0; t <= 1; t += Time.deltaTime/time)
        {
            if (IsInterpolationPaused(id))
            {
                t -= Time.deltaTime / time;
                yield return null;
                continue;
            }

            Color color = new Color(
                Mathf.LerpUnclamped(start.r, goal.r, curve.Evaluate(t)),
                Mathf.LerpUnclamped(start.g, goal.g, curve.Evaluate(t)),
                Mathf.LerpUnclamped(start.b, goal.b, curve.Evaluate(t)),
                Mathf.LerpUnclamped(start.a, goal.a, curve.Evaluate(t))
            );
            property.SetValue(target, color);

            yield return null;
        }

        Color end = new Color(
            Mathf.LerpUnclamped(start.r, goal.r, curve.Evaluate(1)),
            Mathf.LerpUnclamped(start.g, goal.g, curve.Evaluate(1)),
            Mathf.LerpUnclamped(start.b, goal.b, curve.Evaluate(1)),
            Mathf.LerpUnclamped(start.a, goal.a, curve.Evaluate(1))
        );
        property.SetValue(target, end);

        routines.Remove(id);

        if (IsInterpolationPaused(id))
            pausedRoutines.Remove(id);
    }
    #endregion
    
    #region Validation
    private static AnimationCurve ValidateCurve(AnimationCurve curve)
    {
        if (curve == null)
            return linear;
        return curve;
    }

    private static PropertyInfo GetInterpolatablePropertyInfo(Type propertyType, string property, Type expectedType, object target)
    {
        PropertyInfo propertyInfo = propertyType.GetProperty(property);

        if (propertyInfo == null)
            throw new MissingMemberException($"Property '{property}' could not be found in Type {propertyType}");
        if (propertyInfo.GetValue(target).GetType() != expectedType)
            throw new InvalidCastException($"Provided property type does not match expected type");
        if (instance == null)
            throw new NullReferenceException("WiggleWarp object is missing from scene!");

        return propertyInfo;
    }
    #endregion
}
