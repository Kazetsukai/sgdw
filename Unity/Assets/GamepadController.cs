using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadController : MonoBehaviour {

    public float maxSpeed = 2;
    public float turnSpeed = 0.2f;

    public bool snapped = true;

    public Sprite[] directions;

    Vector3 actualPosition;
    Vector3 direction;
    Vector3 targetDirection;

    public Transform shadow;

    float upVel;

	// Use this for initialization
	void Start () {
        direction = Vector3.up;
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            snapped = !snapped;
    }

    // Update is called once per frame
    void FixedUpdate () {

        var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (input.magnitude > 0.1f)
        {
            targetDirection = input.normalized;
            var angle = Vector2.Angle(direction, targetDirection);
            direction = Quaternion.RotateTowards(Quaternion.identity, Quaternion.FromToRotation(direction, targetDirection), turnSpeed + angle * 0.1f) * direction;
            if (Vector2.Angle(direction, targetDirection) > 179)
            {
                // Bump to force a direction
                direction = Quaternion.AngleAxis(2, Vector3.forward) * direction;
            }
            direction.z = 0;


            var intDir = Mathf.RoundToInt(Mathf.Atan2(direction.x, direction.y) / (Mathf.PI / 4) + 4) % 8;

            GetComponent<SpriteRenderer>().sprite = directions[intDir];
        }

        if (upVel <= 0 && actualPosition.z == 0 && Input.GetButton("Fire1"))
        {
            upVel = 2.5f;
        }
        if (actualPosition.z > 0 || upVel > 0)
        {
            actualPosition.z += upVel;
            upVel -= 0.1f;
        }
        else
        {
            upVel = 0;
            actualPosition.z = 0;
        }

        var proportionSpeed = Mathf.Max(0.1f, 120 - Mathf.Abs(Vector3.Angle(direction, targetDirection))) / 120f;
        Debug.Log(Vector3.Angle(direction, targetDirection));

        actualPosition += (Vector3)(input * maxSpeed * proportionSpeed);

        var screenPosition = WorldToScreen(actualPosition);

        Debug.DrawRay(screenPosition, direction * 20);

        

        if (snapped)
            transform.localPosition = new Vector2(Mathf.RoundToInt(screenPosition.x), Mathf.RoundToInt(screenPosition.y));
        else
            transform.localPosition = screenPosition;

        shadow.localPosition = WorldToScreen(new Vector3(actualPosition.x, actualPosition.y, 0)) + Vector2.down * 14;
        shadow.localScale = new Vector3(0.4f - actualPosition.z / 100, 0.4f - actualPosition.z / 100, 1);
    }

    public Vector2 WorldToScreen(Vector3 input)
    {
        return new Vector2(input.x, input.y / Mathf.Sqrt(2) + input.z / Mathf.Sqrt(2));
    }
}
