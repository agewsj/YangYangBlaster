﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryManager : SingleTon<MercenaryManager>
{
    public SpriteRenderer mercenary1;
    public SpriteRenderer mercenary2;

    public Animator mercenaryAnimator1;
    public Animator mercenaryAnimator2;

    public ParticleSystem mercenary1Particle;
    public ParticleSystem mercenary2Particle;

    public float speed;

    float mercenary1AttackSpeed = 0.0f;
    float mercenary1BulletTime = 0.0f;

    float mercenary2AttackSpeed = 0.0f;
    float mercenary2BulletTime = 0.0f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SetLobbyInit()
    {
        mercenary1.gameObject.SetActive(false);
        mercenary2.gameObject.SetActive(false);

        mercenary1Particle.gameObject.SetActive(false);
        mercenary2Particle.gameObject.SetActive(false);
    }

    public void SetInGameInit()
    {
        if (GameDataManager.Instance.userData.mercenaryDataList.Count == 1)
        {
            MercenaryData data_0 = GameDataManager.Instance.userData.mercenaryDataList[0];
            MercenaryResourceData resource_0 = GameDataManager.Instance.userResourceData.mercenaryResourceDataList[(int)data_0.type];

            SetMercenary(0, true, resource_0.catImage, data_0.attackSpeed, resource_0.runtimeAnimator);
            SetMercenary(1, false);
        }
        else if (GameDataManager.Instance.userData.mercenaryDataList.Count >= 2)
        {
            MercenaryData data_0 = GameDataManager.Instance.userData.mercenaryDataList[0];
            MercenaryResourceData resource_0 = GameDataManager.Instance.userResourceData.mercenaryResourceDataList[(int)data_0.type];
            MercenaryData data_1 = GameDataManager.Instance.userData.mercenaryDataList[1];
            MercenaryResourceData resource_1 = GameDataManager.Instance.userResourceData.mercenaryResourceDataList[(int)data_1.type];

            SetMercenary(0, true, resource_0.catImage, data_0.attackSpeed, resource_0.runtimeAnimator);
            SetMercenary(1, true, resource_1.catImage, data_1.attackSpeed, resource_1.runtimeAnimator);
        }

        InitMercenaryPosition();
    }

    public void InitMercenaryPosition()
    {
        mercenary1.transform.position = PlayerManager.Instance.mercenaryPosList[0].position;
        mercenary2.transform.position = PlayerManager.Instance.mercenaryPosList[1].position;
    }

    public void MercenaryMovePoint()
    {
        if (mercenary1.gameObject.activeInHierarchy == true)
        {
            mercenary1.transform.position = Vector3.Lerp(mercenary1.transform.position, PlayerManager.Instance.mercenaryPosList[0].position, Time.deltaTime * speed);
        }

        if (mercenary2.gameObject.activeInHierarchy == true)
        {
            mercenary2.transform.position = Vector3.Lerp(mercenary2.transform.position, PlayerManager.Instance.mercenaryPosList[1].position, Time.deltaTime * speed);
        }
    }

    public void MercenaryShot()
    {
        if (mercenary1.gameObject.activeInHierarchy == true)
        {
            Mercenary1Shot();            
        }

        if (mercenary2.gameObject.activeInHierarchy == true)
        {
            Mercenary2Shot();            
        }
    }

    public void Mercenary1Shot()
    {
        mercenary1BulletTime += Time.deltaTime;

        if (mercenary1BulletTime >= mercenary1AttackSpeed)
        {
            mercenary1BulletTime = 0;

            if (mercenaryAnimator1 != null)
            {
                mercenaryAnimator1.SetTrigger("Attack");
            }            

            BulletManager.Instance.ShotMercenary1Bullet(mercenary1.transform.position);
            mercenary1Particle.Stop();
            mercenary1Particle.Play();
        }
    }

    public void Mercenary2Shot()
    {
        mercenary2BulletTime += Time.deltaTime;

        if (mercenary2BulletTime >= mercenary2AttackSpeed)
        {
            mercenary2BulletTime = 0;

            if (mercenaryAnimator2 != null)
            {
                mercenaryAnimator2.SetTrigger("Attack");
            }
            
            BulletManager.Instance.ShotMercenary2Bullet(mercenary2.transform.position);
            mercenary2Particle.Stop();
            mercenary2Particle.Play();
        }
    }

    public void SetMercenary(int _num, bool isActive, Sprite _sprite = null, float _attackSpeed = 0.0f, RuntimeAnimatorController _animatorRuntime = null)
    {
        if (_num == 0)
        {
            mercenary1.sprite = _sprite;
            mercenary1AttackSpeed = _attackSpeed;
            mercenaryAnimator1.runtimeAnimatorController = _animatorRuntime;
            mercenary1.gameObject.SetActive(isActive);
            mercenary1Particle.gameObject.SetActive(true);
        }
        else if (_num == 1)
        {
            mercenary2.sprite = _sprite;
            mercenary2AttackSpeed = _attackSpeed;
            mercenaryAnimator2.runtimeAnimatorController = _animatorRuntime;
            mercenary2.gameObject.SetActive(isActive);
            mercenary2Particle.gameObject.SetActive(true);
        }
    }
}
