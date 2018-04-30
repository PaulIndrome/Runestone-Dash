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

    public void CreatePool(Transform parent){
        if(originalParentName.Length <= 0){
            keeper = parent;
        } else { 
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
        PoolableParticle p = Instantiate(particleSystems[i % particleSystems.Length], Vector3.zero, Quaternion.identity);
        p.gameObject.SetActive(false);
        p.transform.SetParent(keeper);
        poolQueue.Enqueue(p);
        poolList.Add(p);

        p.SetupPoolableParticle(keeper, poolQueue, poolList);
    }

    public PoolableParticle SpawnFromQueueAndPlay(Transform parent, Vector3 spawnAtPosWorld, Vector3 lookAtPosWorld){
        if(poolQueue.Count <= 0) return null;

        Transform tempParent = parent;

        PoolableParticle poolParticleToSpawn = poolQueue.Dequeue();

        poolParticleToSpawn.gameObject.SetActive(true);
        poolParticleToSpawn.transform.position = spawnAtPosWorld;
        poolParticleToSpawn.transform.SetParent(tempParent);
        lookAtPosWorld.y = spawnAtPosWorld.y;
        poolParticleToSpawn.transform.LookAt(lookAtPosWorld);

        poolParticleToSpawn.PlayFromQueue();

        return poolParticleToSpawn;
    }

    public PoolableParticle SpawnFromListAndPlay(Transform parent, Vector3 spawnAtPosWorld, Vector3 lookAtPosWorld){
        if(poolList.Count <= 0) return null;

        Transform tempParent = parent;

        int randomInt = (lastListSpawn + Random.Range(0, poolList.Count)) % poolList.Count;

        PoolableParticle poolParticleToSpawn = poolList[randomInt];
        while(poolParticleToSpawn.isPlaying){
            randomInt = (lastListSpawn + Random.Range(0, poolList.Count)) % poolList.Count;
            poolParticleToSpawn = poolList[randomInt];
        }

        poolList.RemoveAt(randomInt);

        poolParticleToSpawn.gameObject.SetActive(true);
        poolParticleToSpawn.transform.position = spawnAtPosWorld;
        poolParticleToSpawn.transform.SetParent(tempParent);
        lookAtPosWorld.y = spawnAtPosWorld.y;
        poolParticleToSpawn.transform.LookAt(lookAtPosWorld);

        lastListSpawn = randomInt;

        poolParticleToSpawn.PlayFromList();

        return poolParticleToSpawn;

    }

   





	
}
