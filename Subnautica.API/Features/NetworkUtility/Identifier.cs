namespace Subnautica.API.Features.NetworkUtility
{
    using System;

    using Subnautica.API.Extensions;
    using Subnautica.API.MonoBehaviours;

    using UnityEngine;

    public class Identifier
    {
        /**
         *
         * Benzersiz ID üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GenerateUniqueId()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 22);
        }

        /**
         *
         * Benzersiz ID üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetWorldEntityId(Vector3 position, string uniqueId, bool hash = false)
        {
            return GetWorldEntityId(uniqueId, Network.GetWorldEntityId(position), hash);
        }

        /**
         *
         * Benzersiz ID üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetWorldEntityId(string uniqueId1, string uniqueId2, bool hash = false)
        {
            if (hash)
            {
                return Tools.CreateMD5(string.Format("{0}_{1}", uniqueId1, uniqueId2));
            }
            
            return string.Format("{0}_{1}", uniqueId1, uniqueId2);
        }

        /**
         *
         * Idyi döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetClimbUniqueId(string uniqueId)
        {
            var list = uniqueId.Split('_');
            return list[list.Length - 1];
        }

        /**
         *
         * Nesne kimliğini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetIdentityId(GameObject gameObject, bool autoAdd = true)
        {
            if (gameObject == null)
            {
                return null;
            }

            if (gameObject.TryGetComponent(out UniqueIdentifier identity))
            {
                return identity.Id;
            }

            if (autoAdd)
            {
                return gameObject.AddComponent<ZeroIdentity>().Id;
            }

            return null;
        }

        /**
         *
         * Nesne kimliğini başka nesneye kopyalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void CopyToUniqueIdentifier(GameObject fromGameObject, GameObject toGameObject)
        {
            toGameObject.gameObject.GetComponent<UniqueIdentifier>().Id = this.GetIdentityId(fromGameObject);
        }

        /**
         *
         * Nesne kimliğini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetIdentityId(GameObject gameObject, string newIdentity)
        {
            if (gameObject.TryGetComponent(out UniqueIdentifier identity))
            {
                identity.Id = newIdentity;
            }
            else
            {
                gameObject.AddComponent<ZeroIdentity>().Id = newIdentity;
            }
        }

        /**
         *
         * Oyun nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject GetGameObject(string uniqueId, bool supressMessage = false)
        {
            if (UniqueIdentifier.TryGetIdentifier(uniqueId, out var uniqueIdentifier) && uniqueIdentifier != null)
            {
                return uniqueIdentifier.gameObject;
            }
                
            if (!supressMessage)
            {
                Log.Error(string.Format("Network.Identifier.GetGameObject Not Found. Id: {0}", uniqueId));
            }
            
            return null;
        }

        /**
         *
         * Oyun nesnesinden komponent döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetComponentByGameObject<T>(string uniqueId, bool supressMessage = false)
        {
            if (!UniqueIdentifier.TryGetIdentifier(uniqueId, out var uniqueIdentifier) || uniqueIdentifier == null)
            {
                if (!supressMessage)
                {
                    Log.Error(string.Format("Network.Identifier.GetComponentByGameObject Not Found. Type: {0}, Id: {1}", typeof(T), uniqueId));
                }

                return default(T);
            }

            return uniqueIdentifier.GetComponentInChildren<T>();
        }

        /**
         *
         * Verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose()
        {
        }
    }
}
