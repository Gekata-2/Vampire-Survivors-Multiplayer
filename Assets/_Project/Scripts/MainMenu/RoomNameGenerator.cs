using System;

namespace _Project.Scripts.MainMenu
{
    public class RoomNameGenerator
    {
        private const int MAX_LENGTH = 32;
        private readonly int _nameLength;

        public RoomNameGenerator(int nameLength = 4)
        {
            if (nameLength > MAX_LENGTH)
                nameLength = MAX_LENGTH;

            _nameLength = nameLength;
        }

        public string GetName() 
            => Guid.NewGuid().ToString("N").ToUpper()[.._nameLength];
    }
}