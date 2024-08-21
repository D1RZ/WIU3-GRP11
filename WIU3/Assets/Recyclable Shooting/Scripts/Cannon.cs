using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject GameManager;
    [SerializeField] private RecyclableData[] recyclableDatas;
    [SerializeField] private GameObject shootPosition;
    [SerializeField] private GameObject RecyclableObject;
    private Rigidbody2D RecyclableRigidBody2D;
    [SerializeField] private Image currentRecyclableImage;
    private RecyclableData currentRecyclableData;

    [SerializeField] float maxPower;
    [SerializeField] float maxPowerSpeed;

    private LineRenderer lr;
    private Vector2 AimPosition;
    float shotPower;

    private AudioSource sfxAudioSrc;
    [SerializeField] private AudioClip fireAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        //assign audio source
        sfxAudioSrc = GetComponent<AudioSource>();

        //assign the intial recyclable
        ChangeRecyclableData();

        lr = GetComponent<LineRenderer>();
        AimPosition = Vector2.right;
        shotPower = 0;
        RecyclableRigidBody2D = RecyclableObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GetComponent<RecyclingGameManager>().stopActions || Time.timeScale == 0)
            return;

        if (Input.GetMouseButton(0))
        {
            // Check where the mouse is aiming
            AimPosition = (mousePosition() - transform.position).normalized;

            Debug.Log(AimPosition);

            // Increase the shot power over time
            shotPower = Mathf.Lerp(shotPower, maxPower, maxPowerSpeed * Time.deltaTime);
            //Debug.Log(shotPower);

            Vector2 velocity = AimPosition * shotPower;

            //Vector2 v = (force / RecyclableRigidBody2D.mass) * Time.fixedDeltaTime;


            Vector2[] trajectory = Plot(RecyclableRigidBody2D, shootPosition.transform.position, velocity, 500);
            lr.positionCount = trajectory.Length;
            Vector3[] positions = new Vector3[trajectory.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = trajectory[i];
            }
            lr.SetPositions(positions);

        }
        if (Input.GetMouseButtonUp(0))
        {
            //play audio when fired
            sfxAudioSrc.clip = fireAudioClip;
            sfxAudioSrc.Play();

            //reset the trajectory line
            lr.positionCount = 0;

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
        int randomIndex = UnityEngine.Random.Range(0, recyclableDatas.Length);
        currentRecyclableData = recyclableDatas[randomIndex];
        currentRecyclableImage.GetComponent<Image>().sprite = currentRecyclableData.Image;
    }

    private Vector3 mousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;

        float drag = 1.0f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }



        return results;
    }
}
