using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float moveInput = 0f;
    public bool jumpInput = false;
    public bool defendInput = false;
    public bool attackInput = false;
    public bool weaponInput = false;
    public bool speakInput = false;
    public int weaponState;

    public CharacterController charCtrl;

    void Awake()
    {
        charCtrl = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        CharacterControl();
    }

    public void CharacterControl()
    {
        if (charCtrl.healthPoint <= 0f) { return; }

        //move input
        moveInput = Input.GetAxis("Horizontal");
        float moveInputABS;
        moveInputABS = Mathf.Abs(moveInput);

        //jump input
        jumpInput = Input.GetKeyDown("w");

        //defend input
        defendInput = Input.GetKey("s");

        //attack input
        attackInput = Input.GetKeyDown("k");

        //switch weapon input
        weaponInput = Input.GetKeyDown("i");

        //switch weapon input
        speakInput = Input.GetKeyDown("o");

        if (speakInput)
        {
            charCtrl.Speak();
        }

        if (weaponInput)// switch weapon i
        {
            charCtrl.SwitchWeapon();
        }
        else if (attackInput)// attack k
        {
            charCtrl.Attack();
        }
        else if (moveInput > 0.3f)//when player push d move right
        {
            charCtrl.Move(moveInputABS);
            if (charCtrl.faceDir != CharacterController.direction.right)
            {
                // charCtrl.faceDir = CharacterController.direction.right;
                charCtrl.TurnCharacter(CharacterController.direction.right);
            }
        }
        else if (moveInput < -0.3f)//when player push a move left
        {
            charCtrl.Move(moveInputABS);
            if (charCtrl.faceDir != CharacterController.direction.left)
            {
                // charCtrl.faceDir = CharacterController.direction.left;
                charCtrl.TurnCharacter(CharacterController.direction.left);
            }
        }
        else
        {
            charCtrl.Idle();
        }

        if (jumpInput)//when player push w
        {
            charCtrl.Jump();
        }
        else if (defendInput)//when player push s
        {
            charCtrl.Defend();
        }
        else
        {
            charCtrl.Undefend();
        }

        ResetButton();
    }

    private void ResetButton()
    {
        moveInput = 0f;
        jumpInput = false;
        defendInput = false;
        attackInput = false;
        weaponInput = false;
    }

    public void ButtonJump()
    {
        jumpInput = true;
    }

    public void ButtonMoveLeft()
    {
        moveInput = -1;
    }

    public void ButtonMoveRight()
    {
        moveInput = 1;
    }

    public void ButtonAttack()
    {
        attackInput = true;
    }

    public void ButtonDefend()
    {
        defendInput = true;
    }

    public void ButtonInteract()
    {
        weaponInput = true;
    }
}