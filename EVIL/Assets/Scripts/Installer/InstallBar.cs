using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallBar : MonoBehaviour
{
    [Header("Interpolation")]
    [SerializeField] private int goalWidth;
    [SerializeField] private float rate;
    [SerializeField] private AnimationCurve curve;
    private Vector2 goal;
    private int id = -1;

    [Header("Errror")]
    [SerializeField] private GameObject error;
    private bool errored;

    [Header("References")]
    private Transform canvas;
    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").transform;

        goal = new Vector2(goalWidth, rect.sizeDelta.y);
    }

    void OnEnable()
    {
        if (errored) return;
        
        if (WiggleWarp.DoesInterpolationExist(id))
            WiggleWarp.ResumeInterpolation(id);
        else
            id = WiggleWarp.InterpolateVector2(rect, "sizeDelta", goal, rate, curve, RateMode.speed);
    }

    void Update()
    {
        if (errored) return;

        if (id != -1 && !WiggleWarp.DoesInterpolationExist(id))
            StartCoroutine(SpawnErrors());
    }

    void OnDisable()
    {
        if (errored) return;

        if (WiggleWarp.DoesInterpolationExist(id))
            WiggleWarp.PauseInterpolation(id);
    }

    IEnumerator SpawnErrors()
    {
        errored = true;

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < Random.Range(10, 21); i++)
        {
            int x = Random.Range(-60, 61);
            int y = Random.Range(-37, 38);
            RectTransform errorTransform = Instantiate(error, Vector3.zero, Quaternion.identity, canvas).GetComponent<RectTransform>();
            errorTransform.anchoredPosition = new Vector2(x, y);

            yield return new WaitForSeconds(Random.Range(.05f, .3f));
        }

        yield return new WaitForSeconds(.3f);

        InstanceManager.CreateNewInstance(1);
    }
}
