using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{
    [SerializeField] private RecyclableData[] recyclableDatas;
    [SerializeField] private GameObject shootPosition;
    [SerializeField] private GameObject RecyclableObject;
    [SerializeField] private Image currentRecyclableImage;
    private RecyclableData currentRecyclableData;

    [SerializeField] float maxPower;
    [SerializeField] float maxPowerSpeed;

    private Vector2 AimPosition;
    float shotPower;

    // Start is called before the first frame update
    void Start()
    {
        //assign the intial recyclable
        ChangeRecyclableData();

        AimPosition = Vector2.zero;
        shotPower = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Increase the shot power over time
            shotPower = Mathf.Lerp(shotPower, maxPower, maxPowerSpeed * Time.deltaTime);
            //Debug.Log(shotPower);
        }
        if (Input.GetMouseButtonUp(0))
        {

            // Check where the mouse is aiming
            AimPosition = (mousePosition() - transform.position).normalized;

            // Create new Recyclable at shoot point
            GameObject newRecyclable = Instantiate(RecyclableObject, shootPosition.transform.position, Quaternion.identity);

            // Assign the current Recyclable data to the new Recyclable
            newRecyclable.GetComponent<Recyclable>().data = currentRecyclableData;

            // Shoot the Recyclable in the mouse direction
            newRecyclable.GetComponent<Rigidbody2D>().AddForce(AimPosition * shotPower, ForceMode2D.Impulse);

            // Change the current Recyclable data to a new one
            ChangeRecyclableData();

            // Reset variables and make sure Recyclable cannot be thrown again
            shotPower = 0;
        }
    }

    public void ChangeRecyclableData()
    {
        int randomIndex = Random.Range(0, recyclableDatas.Length);
        currentRecyclableData = recyclableDatas[randomIndex];
        currentRecyclableImage.GetComponent<Image>().sprite = currentRecyclableData.Image;
    }

    private Vector3 mousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
