﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxSneakState : State {

    private CharacterController player;
    private Vector3 direction;
    private float sneakSpeed = 6f;

    private float start;

    public FoxSneakState(Character character) : base(character)
    {
    }

    public override void OnStateEnter()
    {
        player = character.GetComponent<CharacterController>();
        character.frogAnimator.SetBool("Moving", true);
        Character.sneaking = true;
        character.foxAnimator.SetBool("Sneaking", true);
        //character.foxAnimator.Play("Fox_Sneak_NRM", -1, 0.1f);
        start = Time.time;

    }

    public override void Tick()
    {

        direction = forwardtest.forward;
        if (direction == Vector3.zero && Time.time - start > 0.3f)
        {
            character.foxAnimator.speed = 0f;
        } else
        {
            character.foxAnimator.speed = 1f;
        }

        if (!player.isGrounded)
        {
            character.SetState(new FoxFallState(character));
        }
        if (Input.GetAxis("Ability2") == 0f)
        {
            Character.sneaking = false;
            if (Input.GetAxis("Vertical") == 0.0f && Input.GetAxis("Horizontal") == 0.0f)
            {
                character.SetState(new FoxIdleState(character));
            } else
            {
                character.SetState(new FoxMoveState(character));
            }
        }

        if (Input.GetAxis("Jump") != 0.0f && !character.jumped)
        {
            character.SetState(new FoxJumpState(character));
        }
    }

    public override void PhysicsTick()
    {
        character.momentum = Vector3.Lerp(character.momentum, direction * sneakSpeed, 0.08f);

        RaycastHit hit;
        Vector3 newpos = (character.transform.position + (character.momentum.normalized * (player.radius/2)));

        if (Physics.Raycast(newpos, Vector3.down, 1.5f))
        {
            player.Move(character.momentum * Time.fixedDeltaTime);
        }

        else if (Physics.SphereCast(newpos, player.radius / 2, Vector3.down, out hit, 1.2f))
        {
            Vector3 direction = hit.point - character.transform.position;
            direction.y = 0;
            direction.Normalize();
            Vector3 heading = Vector3.Project(character.momentum.normalized, direction) * character.momentum.magnitude;
            player.Move(heading * Time.fixedDeltaTime);
            
        }
        player.Move(Vector3.down * character.gravity * Time.fixedDeltaTime);
    }

    public override void OnStateExit()
    {
        character.foxAnimator.SetBool("Sneaking", false);
        character.foxAnimator.speed = 1f;
    }

    public override void OnColliderHit(ControllerColliderHit hit)
    {
        Vector3 hitNormal = hit.normal;
        bool isGrounded = (Vector3.Angle(Vector3.up, hitNormal) <= player.slopeLimit);
        if (!isGrounded && !player.isGrounded)
        {
            character.SetState(new FoxFallState(character));
        }
        else
        {
        }
    }
}
