
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AddressableLoader
{
	public static async Task<AsyncOperationHandle<T>> LoadAssetAsync<T>(object key) where T : UnityEngine.Object
	{
		var handle = Addressables.LoadAssetAsync<T>(key);
		await handle.Task;

		if (handle.Status != AsyncOperationStatus.Succeeded)
		{
			var exception = handle.OperationException;
			Addressables.Release(handle);
			throw new InvalidOperationException($"Failed to load Addressable asset '{key}'.", exception);
		}

		return handle;
	}

	public static async Task<GameObject> InstantiateAsync(object key, Transform parent = null)
	{
		var handle = Addressables.InstantiateAsync(key, parent);
		await handle.Task;

		if (handle.Status != AsyncOperationStatus.Succeeded)
		{
			var exception = handle.OperationException;
			Addressables.Release(handle);
			throw new InvalidOperationException($"Failed to instantiate Addressable asset '{key}'.", exception);
		}

		return handle.Result;
	}

	public static void ReleaseAsset<T>(AsyncOperationHandle<T> handle) where T : UnityEngine.Object
	{
		if (handle.IsValid())
			Addressables.Release(handle);
	}

	public static bool ReleaseInstance(GameObject instance)
	{
		return instance != null && Addressables.ReleaseInstance(instance);
	}
}
