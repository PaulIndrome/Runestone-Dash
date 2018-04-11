using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableParticle : MonoBehaviour {

    Transform originalParent;
    ParticleSystem ps;
    Queue<PoolableParticle> poolQueue;
    List<PoolableParticle> poolList;

    enum PlayedFrom {
        List, 
        Queue,
        Init
    }

    PlayedFrom lastPlayedFrom = PlayedFrom.Init;

    [HideInInspector] public bool isPlaying {
        get { return ps.isPlaying; }
    }

    public void SetupPoolableParticle(Transform origParent, Queue<PoolableParticle> queue, List<PoolableParticle> list){
        originalParent = origParent;
        poolQueue = queue;
        poolList = list;
    }

    void Awake(){
        ps = GetComponent<ParticleSystem>();
    }

    public void PlayFromQueue(){
        lastPlayedFrom = PlayedFrom.Queue;
        if(ps.isPlaying) return;
        ps.Play();
    }

    public void PlayFromList(){
        lastPlayedFrom = PlayedFrom.List;
        if(ps.isPlaying) return;
        ps.Play();
    }

    public void OnParticleSystemStopped(){
        ReturnParticle();
    }

    public void ReturnParticle(){
        transform.SetParent(originalParent);
        switch(lastPlayedFrom){
            case PlayedFrom.Init: 
                return;
            case PlayedFrom.Queue: 
                poolQueue.Enqueue(this);
                return;
            case PlayedFrom.List:
                poolList.Add(this);
                return;
            default:
                return;
        }
    }


}
