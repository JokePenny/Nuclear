using System;

namespace Nuclear.data.mapeditor
{
    [Serializable]
    class BulletPhysics
    {
        private string _nameBullet;
        private string _nameGun;
        private int _velocity;
        private int _countBullets;
        private int _difficultUse;
        private int _sizeBullet;
        private int _longRange;
        private bool _burstBullet;

        public BulletPhysics(int Velocity, int CountBullets, int DifficultUse, int SizeBullet, int LongRange, bool BurstBullet, string NameBullet, string NameGun)
        {
            _nameBullet = NameBullet;
            _nameGun = NameGun;
            _velocity = Velocity;
            _countBullets = CountBullets;
            _difficultUse = DifficultUse;
            _sizeBullet = SizeBullet;
            _longRange = LongRange;
            _burstBullet = BurstBullet;
        }

        //get
        public string GetNameBullet()
        {
            return _nameBullet;
        }

        public string GetNameGun()
        {
            return _nameGun;
        }

        public int GetLongRange()
        {
            return _longRange;
        }

        public int GetVelocity()
        {
            return _velocity;
        }

        public int GetCountBullets()
        {
            return _countBullets;
        }

        public int GetDifficultUse()
        {
            return _difficultUse;
        }

        public int GetSizeBullet()
        {
            return _sizeBullet;
        }

        public bool GetBurstBullet()
        {
            return _burstBullet;
        }
    }
}