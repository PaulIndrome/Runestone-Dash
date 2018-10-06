using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="ParticlePooler")]
public class ParticlePooler : ScriptableObject {

    [SerializeField] string originalParentName;
    [SerializeField] int poolSize = 10;
    [SerializeField] PoolableParticle[] particleSystems;

    Queue<PoolableParticle> poolQueue;
    List<PoolableParticle> poolList;
    int lastListSpawn = 0;
    Transform keeper;

    //create the pool by instantiating the prefabs in the particleSystems list
    public void CreatePool(Transform parent){
        //if no parentname is set for the ParticlePooler, no new keeper is created
        if(originalParentName.Length <= 0){
            keeper = parent;
        } else {
            //if a new keeper needs to be made, it only gets created once
            keeper = parent.Find(originalParentName);
            if(keeper == null){
                keeper = new GameObject().transform;
                keeper.name = originalParentName;
                keeper.SetParent(parent);
            } 
        }

        poolQueue = new Queue<PoolableParticle>();
        poolList = new List<PoolableParticle>();

        for(int i = 0; i < poolSize; i++){
            CreatePoolObject(keeper, i);
        }
    }

    void CreatePoolObject(Transform keeper, int i){
        //instantiation of prefabs cycles through the array of prefabs
        PoolableParticle p = Instantiate(particleSystems[i % particleSystems.Length], Vector3.zero, Quaternion.identity);
        p.gameObject.SetActive(false);
        p.transform.SetParent(keeper);
        poolQueue.Enqueue(p);
        poolList.Add(p);

        p.SetupPoolableParticle(keeper, poolQueue, poolList);
    }

    //spawning from the queue is faster but keeps the order the prefabs were instantiated in
    public PoolableParticle SpawnFromQueueAndPlay(Transform parent, Vector3 spawnAtPosWorld, Vector3 lookAtPosWorld){
        if(poolQueue.Count <= 0) return null;

        PoolableParticle poolParticleToSpawn = poolQueue.Dequeue();

        poolParticleToSpawn.gameObject.SetActive(true);
        poolParticleToSpawn.transform.position = spawnAtPosWorld;
        //parent can be null, in which case this PoolableParticle will be put at the top of the scene hierarchy
        poolParticleToSpawn.transform.SetParent(parent);
        lookAtPosWorld.y = spawnAtPosWorld.y;
        poolParticleToSpawn.transform.LookAt(lookAtPosWorld);

        poolParticleToSpawn.PlayFromQueue();

        return poolParticleToSpawn;
    }

    //spawning from list is slower but more random than from queue
    //unfortunately, the two methods do not currently check against each other
    //spawning from list is, however, currently never used in game
    public PoolableParticle SpawnFromListAndPlay(Transform parent, Vector3 spawnAtPosWorld, Vector3 lookAtPosWorld){
        if(poolList.Count <= 0) return null;

        //choose a random PoolableParticle that isn't yet playing by incrementing the last index
        //by an amount that prevents landing on the same index again
        int randomInt = (lastListSpawn + Random.Range(1, poolList.Count-1)) % poolList.Count;
        PoolableParticle poolParticleToSpawn = poolList[randomInt];

        //depending on luck and PoolableParticle use, this might take a while
        while(poolParticleToSpawn.isPlaying){
            randomInt = (lastListSpawn + Random.Range(1, poolList.Count-1)) % poolList.Count;
            poolParticleToSpawn = poolList[randomInt];
        }

        poolList.RemoveAt(randomInt);

        poolParticleToSpawn.gameObject.SetActive(true);
        poolParticleToSpawn.transform.position = spawnAtPosWorld;
        //parent can be null, in which case this PoolableParticle will be put at the top of the scene hierarchy
        poolParticleToSpawn.transform.SetParent(parent);
        lookAtPosWorld.y = spawnAtPosWorld.y;
        poolParticleToSpawn.transform.LookAt(lookAtPosWorld);

        lastListSpawn = randomInt;

        poolParticleToSpawn.PlayFromList();

        return poolParticleToSpawn;

    }

   





	
}
