﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager
{
    public interface Driver
    {
        /// <summary>
        /// The list of actions performed
        /// </summary>
        List<Command> CommandList { get; set; }

        /// <summary>
        /// Gets the current study
        /// </summary>
        IStudy Study { get; set; }

        /// <summary>
        /// Navigate to the provided layout
        /// </summary>
        /// <param name="newLayout">The layout to display</param>
        void Navigate(StudyLayout newLayout);

        /// <summary>
        /// Allows operations to be performed on the user interface. This 
        /// enables buttons in the GUI
        /// </summary>
        void EnableOperations();
    }
}
