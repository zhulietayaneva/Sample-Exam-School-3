//A15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Instruments
{
    class Note
    {
        public string Name { get; private set; }
        public char Octave { get; private set; }
        public int Duration { get; private set; }
        public bool isValid => Validate();

        public Note(string name, char octave, int duration)
        {
            Name = name;
            Octave = octave;
            Duration = duration;
        }

        public Note(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return null;
        }


        private bool Validate()
        {
            bool res = true;
            string[] names = { "A", "Bb", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", };

            if (!(names.Contains(this.Name)) || !Char.IsDigit(Octave) || (this.Duration < 1 || this.Duration > 1000))
            {
                res = false;
            }

            return res;
        }
    }

    abstract class Instrument
    {
        public string Name { get; private set; }
        public string Kind { get; private set; }
        public Note Lowest { get; private set; }
        public Note Highest { get; private set; }

        public Instrument(string name, string kind, Note lowest, Note highest)
        {
            Name = name;
            Kind = kind;
            Lowest = lowest;
            Highest = highest;
        }


        public override string ToString()
        {
            return $"{Kind}:{Name}";
        }
    }

    class Aerophone : Instrument
    {
        public string SubKind { get; protected set; }
        public Aerophone(string name, Note lowest, Note highest) : base(name, "Aerophone", lowest, highest)
        {
        }

        public override string ToString()
        {
            return $"{Kind},{SubKind}: {base.Name}";
        }
    }

    class Chordphone : Instrument
    {

        public Chordphone(string name, Note lowest, Note highest) : base(name, "Chordphone", lowest, highest)
        {
        }

    }

    class Brass : Aerophone
    {
        public Brass(string name, Note lowest, Note highest) : base(name, lowest, highest)
        {
            base.SubKind = "Brass";
        }
    }

    class Woodwind : Aerophone
    {
        public Woodwind(string name, Note lowest, Note highest) : base(name, lowest, highest)
        {
            base.SubKind = "Woodwind";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            var instruments = new List<Instrument>();

            for (int i = 0; i < n; i++)
            {
                string[] curr = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                switch (curr[0])
                {
                    case "B":

                        var brass = new Brass(curr[1], new Note(curr[2]), new Note(curr[3]));
                        instruments.Add(brass);
                        break;

                    case "C":
                        var chordphone = new Chordphone(curr[1], new Note(curr[2]), new Note(curr[3]));
                        instruments.Add(chordphone);
                        break;
                    case "W":
                        var woodwind = new Woodwind(curr[1], new Note(curr[2]), new Note(curr[3]));
                        instruments.Add(woodwind);
                        break;
                }

            }

            var orderedInstruments = instruments.OrderBy(x => x.Name).ToList();
            for (int i = 1; i <= orderedInstruments.Count(); i++)
            {
                Console.WriteLine($"{i}. {orderedInstruments[i - 1].ToString()}");
            }





            string[] notesInput = Console.ReadLine().Split(',', StringSplitOptions.RemoveEmptyEntries);
            var notes = new List<Note>();
            foreach (var note in notesInput)
            {
                //C#410,E410,F410,E410,F410,G420,G420,E410,F410,E410,F410,G420,G420,A510,G520,A510,G510,F520,F520,G210,F210,G210,F210,E220,E220
                //      E410,F410,E410,F410,G420,G420,E410,F410,E410,F410,G420,G420,A510,G520,A510,G510,F520,F520,G210,F210,G210,F210,E220,E220
                var octaveIndex = Regex.Match(note, @"\d").Index;
                var durationStartIndex = octaveIndex+1;
                var name = note.Substring(0, octaveIndex);

                var create = new Note(name, note[octaveIndex], int.Parse(note.Substring(durationStartIndex)));
                if (create.isValid)
                {
                    notes.Add(create);
                }
                
            }

            Console.WriteLine(notes.Sum(x=>x.Duration/10));

            var orderedNotes = notes.OrderBy(x=>x.Octave).ThenBy(x=>x.Name);
            string[] Notenames = { "A", "Bb", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", };
            foreach (var instrument in orderedInstruments)
            {
                var octaveIndex = Regex.Match(instrument.Lowest.Name, @"\d").Index;
                var name = instrument.Lowest.Name.Substring(0, octaveIndex);
                var lowest = new Note(name, instrument.Lowest.Name[octaveIndex], 1);

                var octaveIndex2 = Regex.Match(instrument.Highest.Name, @"\d").Index;
                var name2 = instrument.Highest.Name.Substring(0, octaveIndex2);
                var highest = new Note(name2, instrument.Highest.Name[octaveIndex2], 1);

                if (orderedNotes.ElementAtOrDefault(0).Octave>lowest.Octave &&
                    orderedNotes.ElementAtOrDefault(orderedNotes.Count()-1).Octave < highest.Octave)
                {
                    
                        Console.WriteLine(instrument.ToString());
                    
                }
                else if (orderedNotes.ElementAtOrDefault(0).Octave == lowest.Octave &&
                    orderedNotes.ElementAtOrDefault(orderedNotes.Count() - 1).Octave < highest.Octave)
                {

                    if (Array.IndexOf(Notenames, orderedNotes.ElementAtOrDefault(0).Name) >= Array.IndexOf(Notenames, lowest.Name) 
                        )
                    {
                        Console.WriteLine(instrument.ToString());
                    }
                    
                }

                else if (orderedNotes.ElementAtOrDefault(0).Octave > lowest.Octave &&
                    orderedNotes.ElementAtOrDefault(orderedNotes.Count() - 1).Octave == highest.Octave)
                {

                    if (Array.IndexOf(Notenames, orderedNotes.ElementAtOrDefault(0).Name) <= Array.IndexOf(Notenames, highest.Name)
                        )
                    {
                        Console.WriteLine(instrument.ToString());
                    }

                }
                else if (orderedNotes.ElementAtOrDefault(0).Octave == lowest.Octave && orderedNotes.ElementAtOrDefault(orderedNotes.Count() - 1).Octave == highest.Octave )
                {
                    if (Array.IndexOf(Notenames, orderedNotes.ElementAtOrDefault(0).Name) <= Array.IndexOf(Notenames, highest.Name) &&
                        Array.IndexOf(Notenames, orderedNotes.ElementAtOrDefault(0).Name) >= Array.IndexOf(Notenames, lowest.Name)
                       )
                    {
                        Console.WriteLine(instrument.ToString());
                    }
                }


            }
           

        }       
    }
}
