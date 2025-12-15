using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject PogoHitbox;
    [SerializeField] GameObject Spell;
    private InputAction attackAction;
    private InputAction downbuttonAction;
    private InputAction spellAction;
    private PlayerController playerController;
    private Animator animator;
    private bool spellActionEnded = true;
    private List<GameObject> bullets = new List<GameObject>();
    private int bulletsAmount;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        downbuttonAction = InputSystem.actions.FindAction("DownButton");
        spellAction = InputSystem.actions.FindAction("Spell");
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        bulletsAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (!spellActionEnded && !spellAction.inProgress)
        {
            if (!Spell.GetComponent<ThinkingSpell>().HasItems())
            {
                animator.Play("Idle");
            }
            animator.SetBool("Thinking", false);
            spellActionEnded = true;
            Spell.GetComponent<ThinkingSpell>().Fire();
        }

        if (!playerController.IsMoveable())
        {
            return;
        }

        if (playerController.IsBusy())
        {
            return;
        }

        //Attacks
        if (attackAction.inProgress && playerController.IsGrounded()) //Ground shot
        {
            animator.Play("ShotGround");
            StartAttack();
        }
        else if (attackAction.inProgress && downbuttonAction.inProgress) //Pogo
        {
            animator.Play("Pogo");
            StartAttack();
        }
        else if (attackAction.inProgress) //Air shot
        {
            animator.Play("ShotJump");
            StartAttack();
        }

        //Spells
        if (spellAction.inProgress && playerController.IsGrounded())
        {
            animator.SetBool("Thinking", true);
            spellActionEnded = false;
            StartAttack();
            Spell.GetComponent<ThinkingSpell>().Enable();
        }

    }

    public void StartAttack()
    {
        playerController.StartBeingBusy();
    }
    public void EndAttack()
    {
        playerController.StopBeingBusy();
    }

    public void ActivatePogoHitbox()
    {
        PogoHitbox.SetActive(true);
    }
    public void DeactivatePogoHitbox()
    {
        PogoHitbox.SetActive(false);
    }

    public void UseProjectile()
    {
        int bulletIndex = FindBullet();
        bullets[bulletIndex].transform.position = new Vector3(firePoint.position.x, firePoint.position.y, 0);
        bullets[bulletIndex].GetComponent<Bullet>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindBullet()
    {
        for (int i = 0; i < bulletsAmount; i++) {
            if (!bullets[i].activeInHierarchy)
            {
                return i;
            }
        }
        CreateBullet();
        return bulletsAmount-1;
    }
    private void CreateBullet()
    {
        bulletsAmount += 1;
        GameObject newBullet = Instantiate(bulletPrefab);
        bullets.Add(newBullet);
    }

    public void SpawnItem()
    {
        Spell.GetComponent<ThinkingSpell>().SpawnItem();
    }



}
