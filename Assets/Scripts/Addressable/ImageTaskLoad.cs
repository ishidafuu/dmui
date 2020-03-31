using System;
using System.Threading;
using UniRx.Async;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AddressableGetAwaiterExtension
{
    public static UniTask<T>.Awaiter GetAwaiter<T>(this AsyncOperationHandle<T> asset)
    {
        UniTaskCompletionSource<T> completionSource = new UniTaskCompletionSource<T>();
        asset.Completed += (result) =>
        {
            switch (result.Status)
            {
                case AsyncOperationStatus.None:
                    break;
                case AsyncOperationStatus.Succeeded:
                    completionSource.TrySetResult(result.Result);
                    break;
                case AsyncOperationStatus.Failed:
                    completionSource.TrySetException(result.OperationException);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(result.OperationException.ToString());
            }
           // completionSource.TrySetResult(result.Result);
        };
        return completionSource.Task.GetAwaiter();
    }
    public static UniTask<T>.Awaiter GetAwaiter<T>(this AsyncOperationHandle<T> asset,CancellationToken cancellationToken)
    {
        UniTaskCompletionSource<T> completionSource = new UniTaskCompletionSource<T>();
        asset.Completed += (result) =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            switch (result.Status)
            {
                case AsyncOperationStatus.None:
                    break;
                case AsyncOperationStatus.Succeeded:
                    completionSource.TrySetResult(result.Result);
                    break;
                case AsyncOperationStatus.Failed:
                    completionSource.TrySetException(result.OperationException);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(result.OperationException.ToString());
            }
        };
        return completionSource.Task.GetAwaiter();
    }
    public static UniTask<T> ToUniTask<T>(this AsyncOperationHandle<T> enumerator)
    {
        return new UniTask<T>(enumerator.GetAwaiter<T>());
    }
}
