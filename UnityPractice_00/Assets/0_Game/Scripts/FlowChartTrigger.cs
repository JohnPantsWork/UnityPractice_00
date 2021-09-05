using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowChartTrigger : MonoBehaviour
{
    [SerializeField] GameObject flowChart;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            flowChart.SetActive(true);
        }
    }
}
