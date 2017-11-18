using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player:MonoBehaviour {
    ////////////////////////////////////////////////////////////////////////////////
    //                                   TYPES                                    //
    ////////////////////////////////////////////////////////////////////////////////
    struct inputType {
        public float X;
        public float Y;
        public bool attack;
        public bool block;
    }

    delegate inputType inputDelegate();
    delegate IEnumerator attackDelegate();

    //////////////////////////////////////////////////////////////////////////////
    //                               INSTANCE VARIABLES                         //
    //////////////////////////////////////////////////////////////////////////////
    public int playerNumber;

    private inputDelegate inputFunc;
    private attackDelegate currentAttack;
    private attackDelegate[] attacks;
    private Animator anim;
    private bool isAttacking;
    //////////////////////////////////////////////////////////////////////////////
    //                              MAIN METHODS                                //
    //////////////////////////////////////////////////////////////////////////////
    void Start() {
        switch(playerNumber) {
            case 1:
                inputFunc=leftStick;
                break;
            case 2:
                inputFunc=rightStick;
                break;
        }

        anim=GetComponent<Animator>();
        attacks=new attackDelegate[1];
        attacks[0]=basicAttack;
        currentAttack=attacks[0];
    }

    // Update is called once per frame
    void Update() {
        inputType input = inputFunc();
        if(input.attack&&!isAttacking) {
            StartCoroutine(currentAttack());
        }
        else if(input.block) {
            StartCoroutine(block());
        }

        Vector3 direction = new Vector3(-input.Y, 0, -input.X);
        if(direction!=Vector3.zero) {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation=rotation;
        }
        Debug.DrawRay(transform.position, transform.forward, Color.green);

    }

    void changeAttack(int attacktype) {
        anim.SetTrigger("Transform");
        currentAttack=attacks[attacktype];
    }

    //////////////////////////////////////////////////////////////////////////////
    //                            ATTACK TYPE METHODS                           //
    //////////////////////////////////////////////////////////////////////////////
    private IEnumerator block() {
        anim.SetTrigger("Transform");
        yield return 0;
    }
    private IEnumerator basicAttack() {
        anim.SetTrigger("Attack");
        isAttacking=true;

        yield return new WaitForSeconds(0.5f);
        isAttacking=false;
    }
    ///////////////////////////////////////////////////////////////////////////
    //                             INPUT TYPE METHODS                        //
    ///////////////////////////////////////////////////////////////////////////
    private inputType leftStick() {
        inputType toRet = new inputType();
        toRet.X=Input.GetAxis("P1-H");
        toRet.Y=Input.GetAxis("P1-V");
        toRet.attack=Input.GetAxis("P1-A")>0.5;
        toRet.block=Input.GetAxis("P1-B")==1;
        return toRet;
    }
    private inputType rightStick() {
        inputType toRet = new inputType();
        toRet.X=Input.GetAxis("P2-H");
        toRet.Y=Input.GetAxis("P2-V");
        toRet.attack=Input.GetAxis("P2-A")>0.5;
        toRet.block=Input.GetAxis("P2-B")==1;
        return toRet;
    }
}

