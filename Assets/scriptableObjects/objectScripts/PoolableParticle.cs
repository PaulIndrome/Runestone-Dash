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

    public ParticleSystem PS {
        get { return ps; }
        set { 
            if(value.main.stopAction == ParticleSystemStopAction.Callback)
                ps = value;
        }
    }

    //the PoolableParticle knows where it belongs, where its home is...
    public void SetupPoolableParticle(Transform origParent, Queue<PoolableParticle> queue, List<PoolableParticle> list){
        originalParent = origParent;
        poolQueue = queue;
        poolList = list;
    }

    void Awake(){
        //Debug.Log("poolableParticle " + name + " awoke");
        ps = GetComponent<ParticleSystem>();
    }

    public bool PlayFromQueue(){
        //this is a very late check if the called PoolableParticle is already playing
        if(ps.isPlaying && lastPlayedFrom != PlayedFrom.Init) {
            return false;
        }
        lastPlayedFrom = PlayedFrom.Queue;
        ps.Play();
        return true;
    }

    public bool PlayFromList(){
        //this is a very late check if the called PoolableParticle is already playing
        if(ps.isPlaying && lastPlayedFrom != PlayedFrom.Init) {
            return false;
        }
        lastPlayedFrom = PlayedFrom.List;
        ps.Play();
        return true;
    }

    public bool PlayFromQueue(float forTime){
        //this is a very late check if the called PoolableParticle is already playing
        if(ps.isPlaying && lastPlayedFrom != PlayedFrom.Init) {
            return false;
        }
        lastPlayedFrom = PlayedFrom.Queue;
        ps.Play();
        StartCoroutine(PlayForTime(forTime));
        return true;
    }

    public bool PlayFromList(float forTime){
        //this is a very late check if the called PoolableParticle is already playing
        if(ps.isPlaying && lastPlayedFrom != PlayedFrom.Init) {
            return false;
        }
        lastPlayedFrom = PlayedFrom.List;
        ps.Play();
        StartCoroutine(PlayForTime(forTime));
        return true;
    }

    public void Stop(){
        ps.Stop();
    }

    //this is the callback method for the ParticleSystem component's StopAction when set to "Callback"
    public void OnParticleSystemStopped(){
        //Debug.Log("Particle " + name + " has stopped");
        ReturnParticle();
    }

    public void ReturnParticle(){
        //Debug.Log("Particle " + name + " is returning");
        transform.localScale = Vector3.one;
        transform.SetParent(originalParent);
        switch(lastPlayedFrom){
            case PlayedFrom.Queue:
                //Debug.Log("Particle " + name + " last played from Queue");
                poolQueue.Enqueue(this);
                break;
            case PlayedFrom.List:
                //Debug.Log("Particle " + name + " last played from List");
                poolList.Add(this);
                break;
            case PlayedFrom.Init:
                break;
            default:
                break;
        }
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    IEnumerator PlayForTime(float time){
        yield return new WaitForSeconds(time);
        Stop();
    }


}
