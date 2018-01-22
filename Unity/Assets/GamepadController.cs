using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadController : MonoBehaviour {

    public float maxSpeed = 3;
    public float turnSpeed = 0.2f;

    public bool snapped = true;

    Vector3 actualPosition;
    Vector3 direction;
    Vector3 targetDirection;

    public Transform shadow;

    float upVel;
    bool attacking;

    SpriteAnimator animator;

	// Use this for initialization
	void Start () {
        direction = Vector3.up;

        animator = GetComponent<SpriteAnimator>();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            snapped = !snapped;
    }

    // Update is called once per frame
    void FixedUpdate() {

        var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        targetDirection = input.normalized;

        if (Input.GetButton("Fire1"))
        {
            attacking = true;
        }
        if (attacking)
        {
            input = Vector3.Project(input, direction);
        }

        // Update direction
        if (input.magnitude > 0.1f && !attacking)
        {
            var angle = Vector2.Angle(direction, targetDirection);
            
            direction = Quaternion.RotateTowards(Quaternion.identity, Quaternion.FromToRotation(direction, targetDirection), turnSpeed + angle * 0.1f) * direction;
            
            if (Vector2.Angle(direction, targetDirection) > 179)
            {
                // Bump to force a direction
                direction = Quaternion.AngleAxis(2, Vector3.forward) * direction;
            }
            
            direction.z = 0;
        }

        var intDir = Mathf.RoundToInt(Mathf.Atan2(direction.x, direction.y) / (Mathf.PI / 4)) % 8;
        if (intDir < 0) intDir += 8;

        var proportionSpeed = Mathf.Max(0.1f, 120 - Mathf.Abs(Vector3.Angle(direction, targetDirection))) / 120f;

        
        // Move character (at least 1 pixel per frame, otherwise don't bother)
        var movement = (Vector3)(input * maxSpeed * proportionSpeed);
        if (movement.magnitude > 1)
        {
            // Move slower while attacking
            if (attacking)
                movement /= 2;

            actualPosition += movement;
        }

        // Update animation
        if (attacking)
        {
            animator.SetAnimation("attack1_" + intDir * 45, 0.05f, () =>
            {
                attacking = false;
            });
        }
        else if (movement.magnitude > 1)
        {
            animator.SetAnimation("run_" + intDir * 45);
        }
        else
        {
            animator.SetAnimation("idle_" + intDir * 45);
        }
        

        // Jumping
        /*if (upVel <= 0 && actualPosition.z == 0 && Input.GetButton("Fire1"))
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
        }*/

        // Move to screen pos
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
