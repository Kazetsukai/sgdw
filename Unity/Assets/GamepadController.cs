using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadController : MonoBehaviour {

    public float maxSpeed = 2;
    public float turnSpeed = 0.2f;

    public bool snapped = true;

    public Sprite[] directions;

    Vector2 actualPosition;
    Vector3 direction;
    Vector3 targetDirection;

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

        var proportionSpeed = Mathf.Max(0.1f, 120 - Mathf.Abs(Vector3.Angle(direction, targetDirection))) / 120f;
        Debug.Log(Vector2.Angle(direction, targetDirection));

        actualPosition += input * maxSpeed * proportionSpeed;

        Debug.DrawRay(actualPosition, direction * 20);

        if (snapped)
            transform.localPosition = new Vector2(Mathf.RoundToInt(actualPosition.x), Mathf.RoundToInt(actualPosition.y));
        else
            transform.localPosition = actualPosition;

    }
}
