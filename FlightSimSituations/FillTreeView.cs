using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FlightSimSituations
{
    public class Situations
    {
        static readonly List<Tuple<string, string>> crewSituations = new List<Tuple<string, string>>
        {
            new Tuple<string, string> ( "First Officer incapacitated", "crew_fo_incapacitated" ),
            new Tuple<string, string> ( "A member of the cabin crew feel unwell", "crew_cabin_sick" ),
            new Tuple<string, string> ( "A member of the cabin crew has fainted", "crew_cabin_fainted" ),
        };

        static readonly List<Tuple<string, string>> emergencySituations = new List<Tuple<string, string>>
        {
            new Tuple<string, string> ( "Lavatory fire", "emergency_fire_lavatory" ),
            new Tuple<string, string> ( "Samsung Galaxy Note 7 is onboard", "emergency_fire_samsung_note_seven" ),
            new Tuple<string, string> ( "Rapid decompression", "emergency_rapid_decompression" ),
        };
        static readonly List<Tuple<string, string>> passengerSituations = new List<Tuple<string, string>>
        {
            new Tuple<string, string> ( "Pax feels unwell", "pax_unwell" ),
            new Tuple<string, string> ( "Pax is suffering from an unknown medical emergency", "pax_medical_emergency" ),
            new Tuple<string, string> ( "Pax is violent", "pax_violent" ),
            new Tuple<string, string> ( "Pax is drunk", "pax_drunk" ),
            new Tuple<string, string> ( "Pax is being abusive", "pax_abusive" ),
            new Tuple<string, string> ( "Pax is attempting to enter the cockpit", "pax_hijack" ),
            new Tuple<string, string> ( "Pax has died", "pax_dead" ),
            new Tuple<string, string> ( "Pax is unconscious", "pax_unconscious" ),
            new Tuple<string, string> ( "Pax has a weapon", "pax_armed_and_dangerous" ),
            new Tuple<string, string> ( "Pax had a panic attack", "pax_panic_attack" ),
        };

        static readonly List<Tuple<string, string>> systemSituations = new List<Tuple<string, string>>
        {
            new Tuple<string, string> ( "Engine vibrations outside of safe range", "system_eng_vibrations_above_maximum" ),
        };

        public static readonly List<Tuple<string, List<Tuple<string, string>>>> allSituations = new List<Tuple<string, List<Tuple<string, string>>>>
        {
            new Tuple<string, List<Tuple<string, string>>> ( "Generic Emergencies", emergencySituations ),
            new Tuple<string, List<Tuple<string, string>>> ( "Passenger", passengerSituations ),
            new Tuple<string, List<Tuple<string, string>>> ( "Aircraft Warnings and Failures", systemSituations ),
            new Tuple<string, List<Tuple<string, string>>> ( "Onboard Crew", crewSituations ),
        };

        public static void PopulateTreeView(TreeView tree, List<Tuple<string, List<Tuple<string, string>>>> allnodes)
        {
            foreach (Tuple<string, List<Tuple<string, string>>> category in allnodes)   // For every category...
            {
                var categoryTitle = category.Item1;                                     // Get the category name
                var itemsInCategory = category.Item2;                                   // Get the items in the category

                var tvCategory = new TreeViewItem                                       // Create a new TreeViewItem for the category parent
                {
                    Header = categoryTitle,
                };

                foreach (Tuple<string, string> item in itemsInCategory)                 // For each item in the category...
                {
                    var tvItem = new CheckBox                                           // Create a new TreeViewItem (child)
                    {
                        Content = item.Item1,                                           // Set the text to the item's text
                        Name = item.Item2,                                              // Set the name to the item's ID
                    };
                    tvItem.Checked += delegate { MainWindow.hasChangeBeenMadeSinceLastSave = true; };

                    tvCategory.Items.Add(tvItem);                                       // Add it to the category (parent)
                }

                tree.Items.Add(tvCategory);                                             // Add the category (parent) to the TreeView (root)
            }
        }
    }
}
