using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager
{
    class FourIterator : IEnumerator<Study>
    {

        private List<Image>.Enumerator _enumerator;

        private Study _study;

        /// <summary>
        /// Creates a new Four Iterator for the given study
        /// </summary>
        /// <param name="s">The study to iterate over</param>
        public FourIterator(Study s)
        {
            _enumerator = s.GetEnumerator();
            _current = new List<Image>();
            _study = s;
            _position = -1;
        }

        private List<Image> _current;
        /// <summary>
        /// The current list of images
        /// </summary>
        public List<Image> Current
        {
            get
            {
                return _current;
            }
            private set;
        }

        private int _position;
        /// <summary>
        /// The position during iteration
        /// </summary>
        public int Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (_position != value)
                {
                    if (_position > value)
                    {
                        Reset();
                    }

                    while (_position < value - 3)
                    {
                        if (!MoveNext())
                        {
                            throw new IndexOutOfRangeException();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Moves to the next image in the study
        /// </summary>
        /// <returns>true if move was successful, false if the move fails</returns>
        public bool MoveNext()
        {
            if (_enumerator.MoveNext())
            {
                _current.Clear();
                _current.Add(_enumerator.Current);
                _position++;
            }
            else
                return false;

            for (int i = 0; i < 3; i++)
            {
                if (_enumerator.MoveNext())
                {
                    _current.Add(_enumerator.Current);
                    _position++;
                }
                else
                    break;
            }
            return true;
        }

        /// <summary>
        /// Moves to the previous image in the study
        /// </summary>
        /// <returns>true if move was successful, false if the move fails</returns>
        public bool MovePrev()
        {
            if (Position <= 0)
            {
                return false;
            }
            else
            {
                Position = Position - 1;
                return true;
            }
        }

        /// <summary>
        /// Resets the enumerator to before the first image in the study
        /// </summary>
        public void Reset()
        {
            _enumerator = _study.GetEnumerator();
            _position = -1;
            _current.Clear();
        }

    }
}
