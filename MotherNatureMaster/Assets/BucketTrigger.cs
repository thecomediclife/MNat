using UnityEngine;
using System.Collections;

public class BucketTrigger : MonoBehaviour {
    public int ballCounter = 0;
    public Transform bottomBoundary;

    public Material whiteMat, blueMat, greenMat, redMat;

    void OnTriggerEnter(Collider other)
    {
        if (ballCounter < 3)
        {
            ballCounter++;
        } else
        {
            ballCounter = 0;
            bottomBoundary.GetComponent<Collider>().enabled = false;
        }

        if (ballCounter == 0)
        {
            this.transform.parent.GetComponent<Renderer>().material = whiteMat;
        } else if (ballCounter == 1)
        {
            this.transform.parent.GetComponent<Renderer>().material = blueMat;
        } else if (ballCounter == 2)
        {
            this.transform.parent.GetComponent<Renderer>().material = redMat;
        } else if (ballCounter == 3)
        {
            this.transform.parent.GetComponent<Renderer>().material = greenMat;
        }
    }
}
