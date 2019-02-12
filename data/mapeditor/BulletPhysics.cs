using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuclear.data.mapeditor
{
    [Serializable]
    class BulletPhysics
    {
        private string _nameBullet;
        private int _velocity;
        private int _numberOfBullets;
        private int _difficultUse;
        private int _sizeBullet;

        public BulletPhysics(int Velocity, int NumberOfBullets, int DifficultUse, int SizeBullet, string NameBullet)
        {
            _nameBullet = NameBullet;
            _velocity = Velocity;
            _numberOfBullets = NumberOfBullets;
            _difficultUse = DifficultUse;
            _sizeBullet = SizeBullet;
        }

        public void SetNameBullet(string A)
        {
            _nameBullet = A;
        }

        public void SetVelocity(int A)
        {
            _velocity = A;
        }

        public void SetNumberOfBullets(int A)
        {
            _numberOfBullets = A;
        }

        public void SetDifficultUse(int A)
        {
            _difficultUse = A;
        }

        public void SetSizeBullet(int A)
        {
            _sizeBullet = A;
        }

        //get

        public string GetNameBullet()
        {
            return _nameBullet;
        }

        public int GetVelocity()
        {
            return _velocity;
        }

        public int GetNumberOfBullets()
        {
            return _numberOfBullets;
        }

        public int GetDifficultUse()
        {
            return _difficultUse;
        }

        public int GetSizeBullet()
        {
            return _sizeBullet;
        }
    }
}
