using Flunt.Notifications;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.ViewModels.CategoryViewModels
{
    public class EditorCategoryViewModel : Notifiable, IValidatable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public void Validate()
        {
            AddNotifications(
                    new Contract()
                        .HasMaxLen(Title, 120, "Title", "O título deve conter até 120 caracteres")
                        .HasMinLen(Title, 3, "Title", "O título deve conter pelo menos 3 caracteres")
                        .HasMaxLen(Description, 300, "Description", "A descrição deve conter até 300 caracteres")
                        .HasMinLen(Description, 10, "Description", "A descrição deve conter pelo menos 10 caracteres")
                );
        }
    }
}
