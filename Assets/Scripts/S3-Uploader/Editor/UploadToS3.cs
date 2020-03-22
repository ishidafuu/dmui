#pragma warning disable 4014
using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using UnityEditor;
using UnityEngine;

namespace DM
{
    public class UploadToS3 : EditorWindow
    {
        private string m_Region;
        private string m_BucketName;
        private string m_IamAccessKeyId;
        private string m_IamSecretKey;
        private const string m_ServerDataDir = "ServerData";
        private RegionEndpoint m_BucketRegion;

        [MenuItem("Tools/Addressable Content Uploader (S3)")]
        private static void Init()
        {
            var window = (UploadToS3)GetWindow(typeof(UploadToS3));
            window.m_Region = EditorPrefs.GetString("UploadToS3_region", "us-east-1");
            window.m_BucketName = EditorPrefs.GetString("UploadToS3_bucketName", "");
            window.m_IamAccessKeyId = EditorPrefs.GetString("UploadToS3_iamAccessKeyId", "");
            window.m_IamSecretKey =
                EditorPrefs.GetString("UploadToS3_iamSecretKey", "");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            if (GUILayout.Button("Upload content"))
            {
                Upload();
            }
            else if (GUILayout.Button("Upload content with Build"))
            {
                UnityEditor.AddressableAssets.Settings.AddressableAssetSettings.BuildPlayerContent();
                Upload();
            }
        }

        private void UpdateValues()
        {
            m_Region = EditorGUILayout.TextField("Region", m_Region);
            m_BucketName = EditorGUILayout.TextField("Bucket name", m_BucketName);
            m_IamAccessKeyId = EditorGUILayout.TextField("IAM Access Key Id", m_IamAccessKeyId);
            m_IamSecretKey = EditorGUILayout.TextField("IAM Secret Key", m_IamSecretKey);
        }

        private void Upload()
        {
            UpdateValues();
            EditorPrefs.SetString("UploadToS3_region", m_Region);
            EditorPrefs.SetString("UploadToS3_bucketName", m_BucketName);
            EditorPrefs.SetString("UploadToS3_iamAccessKeyId", m_IamAccessKeyId);
            EditorPrefs.SetString("UploadToS3_iamSecretKey", m_IamSecretKey);
            m_BucketRegion = RegionEndpoint.GetBySystemName(m_Region);
            InitiateTask();
        }        
        
        public static async void UploadCli()
        {
            string[] args = Environment.GetCommandLineArgs();
            string region = string.Empty;
            string bucketName = string.Empty;
            string iamAccessKeyId= string.Empty;
            string iamSecretKey = string.Empty;
            
            int len = args.Length;
            for(int i = 0; i < len; ++i )
            {
                switch( args[i] )
                {
                    case "/region":
                        region = args[i];
                        break;
                    case "/bucketName":
                        bucketName = args[i];
                        break;
                    case "/iamAccessKeyId":
                        iamAccessKeyId = args[i];
                        break;
                    case "/iamSecretKey":
                        iamSecretKey = args[i];
                        break;
                }
            }
            
            await UploadAsync(RegionEndpoint.GetBySystemName(region),
                bucketName, iamAccessKeyId, iamSecretKey, m_ServerDataDir);
        }

        private async Task InitiateTask()
        {
            await UploadAsync(m_BucketRegion, m_BucketName, m_IamAccessKeyId, m_IamSecretKey, m_ServerDataDir);
        }

        private static async Task UploadAsync(RegionEndpoint bucketRegion, string bucketName, string iamAccessKeyId,
            string iamSecretKey, string path)
        {
            try
            {
                Debug.Log("Starting upload...");
                var credentials = new BasicAWSCredentials(iamAccessKeyId, iamSecretKey);
                var s3Client = new AmazonS3Client(credentials, bucketRegion);
                var transferUtility = new TransferUtility(s3Client);
                var transferUtilityRequest = new TransferUtilityUploadDirectoryRequest
                {
                    BucketName = bucketName,
                    Directory = path,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    CannedACL = S3CannedACL.PublicRead,
                    SearchOption = SearchOption.AllDirectories,
                };
                
                Debug.Log("Upload completed!");
                await transferUtility.UploadDirectoryAsync(transferUtilityRequest);
                Debug.Log("Upload completed!");
            }
            catch (AmazonS3Exception e)
            {
                Debug.LogError("Error encountered on server when writing an object: " + e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError("Unknown encountered on server when writing an object: " + e.Message);
            }
        }
    }
}
#pragma warning restore 4014