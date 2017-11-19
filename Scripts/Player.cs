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
    public int maxHealth;
    public float speed;
    public int currentHealth;

    public GameObject opponent1;
    public GameObject opponent2;
    public GameObject[] modes;

    public GameObject tankShootPoint;
    public GameObject shell;

    public GameObject deathParticles;

    private inputDelegate inputFunc;
    private attackDelegate currentAttack;
    private attackDelegate[] attacks;
    private Animator anim;
    private bool isAttacking;
    private Rigidbody rb;
    private bool grouned = true;
    private bool invuln;
    private bool changingmodes;

    private GameObject currentMode;
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
            case 3:
                inputFunc = leftStick2;
                break;
            case 4:
                inputFunc = rightStick2;
                break;
        }

        currentMode = modes[0];
        anim=currentMode.GetComponent<Animator>();
        attacks=new attackDelegate[2];
        attacks[0]=basicAttack;
        attacks[1] = tankAttac;

        currentAttack=attacks[0];
        rb=GetComponent<Rigidbody>();
        currentHealth=maxHealth;
    }

    // Update is called once per frame
    void Update() {
        inputType input = inputFunc();
       // Debug.Log(input.X + ", " + input.Y);
        if(input.attack && !isAttacking) {
            StartCoroutine(currentAttack());
        }
        else if(input.block && !isAttacking) {
            StartCoroutine(block());
        }

        Vector3 direction = new Vector3(-input.Y, 0, -input.X);
        //Debug.Log(direction);
        if(direction!=Vector3.zero && !changingmodes) {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation=rotation;
            if(grouned) {
                
                rb.AddForce(Quaternion.Euler(0, -90, 0)*direction*speed*10);
                
            }
        }
        anim.SetFloat("Speed", rb.velocity.magnitude);
    }

    void changeMode(int newMode) {
        StartCoroutine("modeHelper", newMode);
    }
    IEnumerator modeHelper(int newMode) {
        anim.SetTrigger("Transform");
        isAttacking = true;
        changingmodes = true;
        yield return new WaitForSeconds(2f);
        currentAttack=attacks[newMode];
        currentMode.SetActive(false);
        currentMode = modes[newMode];
        currentMode.SetActive(true);
        anim = currentMode.GetComponent<Animator>();
        isAttacking = false;
        changingmodes = false;
    }

    public void applyDamage(int dmg) {
        if(dmg > 1 || !invuln) {
            currentHealth = currentHealth - dmg;
            Debug.Log(playerNumber+" "+currentHealth + ", " + dmg);
            if (currentHealth <= 0) {
                onDeath();
            }
        }
        else if(invuln) {
            Debug.Log("BLOCKED!");
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag =="Ground") {
            grouned=true;
        }
    }
    private void OnCollisionExit(Collision collision) {
        if(collision.gameObject.tag=="Ground") {
            grouned=false;
        }
    }
    private void resetBase() {
        currentAttack = attacks[0];
        currentMode.SetActive(false);
        currentMode = modes[0];
        currentMode.SetActive(true);
        anim = currentMode.GetComponent<Animator>();
    }

    public void onDeath(){
        for (int i = 0; i < 10; i++)
        {
            Instantiate(deathParticles, transform.position, Random.rotation);
        }
        Destroy(gameObject);

    }
    //////////////////////////////////////////////////////////////////////////////
    //                            ATTACK TYPE METHODS                           //
    //////////////////////////////////////////////////////////////////////////////
    private IEnumerator block() {
        anim.SetTrigger("Block");
        isAttacking=true;
        invuln = true;

        yield return new WaitForSeconds(0.5f);
        isAttacking=false;
        invuln = false;
    }
    private IEnumerator basicAttack() {
        anim.SetTrigger("Attack");
        isAttacking=true;

        yield return new WaitForSeconds(0.2f);
        if(opponent1 != null) {
            Vector3 directionToTarget = transform.position - opponent1.transform.position;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            float distance = directionToTarget.magnitude;
            Debug.Log(angle + ", " + distance);
            if(Mathf.Abs(angle) < 180 && distance < 5) {
                opponent1.SendMessage("applyDamage", 1);
            }
        }
        if(opponent2 != null) {
            Vector3 directionToTarget = transform.position - opponent2.transform.position;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            float distance = directionToTarget.magnitude;
            if(Mathf.Abs(angle) < 90 && distance < 0) {
                opponent2.SendMessage("applyDamage", 1);
            }
        }

        yield return new WaitForSeconds(0.3f);

        isAttacking=false;
    }
    private IEnumerator tankAttac() {
        Debug.Log("he attac");
        anim.SetTrigger("Attack");
        isAttacking = true;
        changingmodes = true;
        Destroy(Instantiate(shell, tankShootPoint.transform.position, tankShootPoint.transform.rotation), 10f);
        yield return new WaitForSeconds(1f);
        resetBase();
        isAttacking = false;
        changingmodes = false;
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
    private inputType leftStick2() {
        inputType toRet = new inputType();
        toRet.X = Input.GetAxis("P3-H");
        toRet.Y = Input.GetAxis("P3-V");
        toRet.attack = Input.GetAxis("P3-A") > 0.5;
        toRet.block = Input.GetAxis("P3-B") == 1;
        return toRet;
    }
    private inputType rightStick2() {
        inputType toRet = new inputType();
        toRet.X = Input.GetAxis("P4-H");
        toRet.Y = Input.GetAxis("P4-V");
        toRet.attack = Input.GetAxis("P4-A") > 0.5;
        toRet.block = Input.GetAxis("P4-B") == 1;
        return toRet;
    }
}

