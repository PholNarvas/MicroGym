namespace MicroGym.Client.Pages.HomeDashBoard
{
    public partial class HomeDashBoard
    {
        private void OnSelectCategory(int categoryId)
        {
            selectedCategory = selectedCategory == categoryId ? 0 : categoryId;
        }

        private void OpenAddModal()  => showAddModal = true;
        private void CloseAddModal() => showAddModal = false;
    }
}
