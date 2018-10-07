using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableParticle : MonoBehaviour {

    Transform originalParent;
    ParticleSystem ps;
    Queue<PoolableParticle> poolQueue;
    List<PoolableParticle> poolList;

    //remember the method used to spawn this PoolableParticle
    //this will be removed once the ParticlePooler spawn methods check against 
    //each other for already active PoolableParticles
    enum PlayedFrom {
        List, 
        Queue,
        Init
    }

    PlayedFrom lastPlayedFrom = PlayedFrom.Init;

    [HideInInspector] public bool isPlaying {
        get { return ps.isPlaying; }
    }

    //the PoolableParticle knows where it belongs, where its home is...
    public void SetupPoolableParticle(Transform origParent, Queue<PoolableParticle> queue, List<PoolableParticle> list){
        originalParent = origParent;
        poolQueue = queue;
        poolList = list;
    }

    void Awake(){
        ps = GetComponent<ParticleSystem>();
    }

    public bool PlayFromQueue(){
        //this is a very late check if the called PoolableParticle is already playing
        if(ps.isPlaying) return false;
        lastPlayedFrom = PlayedFrom.Queue;
        ps.Play();
        return true;
    }

    public bool PlayFromList(){
        //this is a very late check if the called PoolableParticle is already playing
        if(ps.isPlaying) return false;
        lastPlayedFrom = PlayedFrom.List;
        ps.Play();
        return true;
    }

    //this is the callback method for the ParticleSystem component's StopAction when set to "Callback"
    public void OnParticleSystemStopped(){
        ReturnParticle();
    }

    public void ReturnParticle(){
        transform.SetParent(originalParent);
        transform.localScale = Vector3.one;
        switch(lastPlayedFrom){
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
