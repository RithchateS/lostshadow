using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;

public class Hider : MonoBehaviour
{
    private SpriteRenderer _mySpriteRenderer;
    private GameObject _player;
    private float _distXtoPlayer;
    private float _distYtoPlayer;
    private PlayerController _playerController;
    private Animator _myAnimator;

    void Start()
    {
        _myAnimator = GetComponent<Animator>();
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player");
            _playerController = _player.GetComponent<PlayerController>();
        }
        DistanceToPlayer();
        Interactable();
    }

    void DistanceToPlayer()
    {
        var position = _player.transform.position;
        var position1 = transform.position;
        _distXtoPlayer = position.x - position1.x;
        _distYtoPlayer = position.y - position1.y;
    }

    void Interactable()
    {
        if (_playerController.GetIsHiding())
        {
            _myAnimator.SetBool("isInteracting", true);
        }
        else
        {
            _myAnimator.SetBool("isInteracting", false);
        }

        if (_playerController.GetIsHiding())
        {
            _myAnimator.SetBool("isInteractable", false);
        }
    }

    public void ActivateInteract()
    {
        _myAnimator.SetBool("isInteractable", true);
    }

    public void DeactivateInteract()
    {
        _myAnimator.SetBool("isInteractable", false);
    }
}
