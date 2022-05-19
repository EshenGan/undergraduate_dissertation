using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureBox : MonoBehaviour
{
    //[SerializeField]private GameObject self;

    private void Start()
    {


        //checkOverlap(this.gameObject);
    }


/*    public void checkOverlap(GameObject Object)
    {
        //Debug.Log("check for overlap");
        //check overlap
        bool isOverlap = Physics.CheckBox(Object.transform.position,
                                            Object.transform.localScale / 2,
                                            Quaternion.identity);
        //Debug.Log(Object.transform.position);
        if (isOverlap)
        {
            //Debug.Log("overlap");
            Object.transform.localPosition = new Vector3(Random.Range(-10f, 18f), 0.5f, Random.Range(0f, 27f));
            //Debug.Log("generated new position");
            checkOverlap(Object);


        }
    }*/

    /*    private void OnTriggerEnter(Collider other)
        {
            //using colliders to prevent overlapping of boxes  with the walls/themselves
            if (other.gameObject.CompareTag("treasure"))
            {
                Debug.Log("shouldnt overlap with each other");
                transform.position = new Vector3(Random.Range(-10f, 18f), 0.5f, Random.Range(0f, 27f));
            }

            if (other.gameObject.CompareTag("wall"))
            {
                Debug.Log("shouldnt overlap with walls");
                transform.position = new Vector3(Random.Range(-10f, 18f), 0.5f, Random.Range(0f, 27f));
            }
        }*/

    /*    public bool isOverlap1;
        public bool isOverlap2;
        public bool isOverlap3;
        public bool isOverlap4;

        private void Start()
        {
            //overlap check
            isOverlap1 = Physics.CheckBox(transform.InverseTransformVector(new Vector3(0.525f, 0f, 0f)), transform.InverseTransformVector(new Vector3(0.05f, 0.9f, 1f)), Quaternion.Euler(transform.InverseTransformVector(new Vector3(0f, 0f, 0f))));
            isOverlap2 = Physics.CheckBox(transform.InverseTransformVector(new Vector3(-0.525f, 0f, 0f)), transform.InverseTransformVector(new Vector3(0.05f, 0.9f, 1f)), Quaternion.Euler(transform.InverseTransformVector(new Vector3(0f, 0f, 0f))));
            isOverlap3 = Physics.CheckBox(transform.InverseTransformVector(new Vector3(0f, 0f, 0.525f)), transform.InverseTransformVector(new Vector3(0.05f, 0.9f, 1f)), Quaternion.Euler(transform.InverseTransformVector(new Vector3(0f, 90f, 0f))));
            isOverlap4 = Physics.CheckBox(transform.InverseTransformVector(new Vector3(0f, 0f, -0.525f)), transform.InverseTransformVector(new Vector3(0.05f, 0.9f, 1f)), Quaternion.Euler(transform.InverseTransformVector(new Vector3(0f, 90f, 0f))));
        }*/
    /*    private void Update()
        {
            //overlap check
            isOverlap1 = Physics.CheckBox(transform.InverseTransformVector(new Vector3(0.525f,0f,0f)), transform.InverseTransformVector(new Vector3(0.05f, 0.9f, 1f)), Quaternion.Euler(transform.InverseTransformVector(new Vector3(0f, 0f, 0f))));
            isOverlap2 = Physics.CheckBox(transform.InverseTransformVector(new Vector3(-0.525f, 0f, 0f)), transform.InverseTransformVector(new Vector3(0.05f, 0.9f, 1f)) , Quaternion.Euler(transform.InverseTransformVector(new Vector3(0f, 0f, 0f))));
            isOverlap3 = Physics.CheckBox(transform.InverseTransformVector(new Vector3(0f, 0f, 0.525f)), transform.InverseTransformVector(new Vector3(0.05f, 0.9f, 1f)) , Quaternion.Euler(transform.InverseTransformVector(new Vector3(0f, 90f, 0f))));
            isOverlap4 = Physics.CheckBox(transform.InverseTransformVector(new Vector3(0f, 0f, -0.525f)), transform.InverseTransformVector(new Vector3(0.05f, 0.9f, 1f)) , Quaternion.Euler(transform.InverseTransformVector(new Vector3(0f, 90f, 0f))));

        }*/

}
