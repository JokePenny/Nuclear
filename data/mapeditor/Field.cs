using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuclear.data.mapeditor
{
    [Serializable]
    public class Field
    {
        private string _nameMap;
        private int[,] _ImageIDArray;
        private int[,] _TriggerIDArray;
        private int _sizeArrayX;
        private int _sizeArrayY;

        public Field(int sizeX, int sizeY, string nameFile)
        {
            _ImageIDArray = new int[sizeX, sizeY];
            _TriggerIDArray = new int[sizeX, sizeY];
            _sizeArrayX = sizeX;
            _sizeArrayY = sizeY;
            _nameMap = nameFile;
        }

        public void SetImageIDArray(int x, int y, int A)
        {
            _ImageIDArray[x, y] = A;
        }

        public void SetTriggerIDArray(int x, int y, int A)
        {
            _ImageIDArray[x, y] = A;
        }

        public void SetSizeArrayX(int A)
        {
            _sizeArrayX = A;
        }

        public void SetSizeArrayY(int A)
        {
            _sizeArrayY = A;
        }

        public void SetName(string A)
        {
            _nameMap = A;
        }

        //get

        public int GetImageIDArray(int x, int y)
        {
            return _ImageIDArray[x, y];
        }

        public int GetTriggerIDArray(int x, int y)
        {
            return _ImageIDArray[x, y];
        }

        public int GetSizeArrayX()
        {
            return _sizeArrayX;
        }

        public int GetSizeArrayY()
        {
            return _sizeArrayY;
        }

        public string GetName()
        {
            return _nameMap;
        }

    }
}
