using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Client.ViewModels.Chat.ViewModels
{
    /// <summary>
    /// Vista-Modelo del panel de salas del chat. Permite realizar operaciones
    /// para registrarlas y notificar a la vista del panel de salas del chat.
    /// </summary>
    public partial class RoomsPanelViewModel : ViewModelBase
    {
        #region Eventos

        /// <summary>
        /// Ocurre cuando una sala del panel es seleccionada.
        /// </summary>
        public event Action<ChatRoomViewModel?>? RoomSelected;

        #endregion

        #region Contexto

        /// <summary>
        /// La sala seleccionada.
        /// </summary>
        [ObservableProperty]
        private ChatRoomViewModel? _selectedRoom;

        /// <summary>
        /// Lista de salas en el panel.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ChatRoomViewModelBase> _rooms = [];

        #endregion

        #region Campos

        /// <summary>
        /// Permite acceder a la entidad de vista-modelo de las salas del panel
        /// por medio de su nombre.
        /// </summary>
        private Dictionary<string, ChatRoomViewModelBase> _roomnameToRoomMap = [];

        #endregion

        #region Acceso público

        /// <summary>
        /// Añade una sala al panel.
        /// </summary>
        /// <param name="room">La sala.</param>
        /// <returns><see langword="true"/> si la sala no formaba parte del panel
        /// y pudo ser añadida, y <see langword="false"/> en otro caso.</returns>
        public bool AddRoom(ChatRoomViewModelBase room)
        {
            if (!Rooms.Contains(room))
            {
                Rooms.Add(room);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remueve una sala del panel por su nombre, si la encuentra.
        /// </summary>
        /// <param name="roomname">Nombre de la sala.</param>
        /// <returns><see langword="true"/> si la sala estaba en el panel y pudo
        /// ser removida, y <see langword="false"/> en otro caso.</returns>
        public bool RemoveRoom(string roomname)
        {
            if (_roomnameToRoomMap.Remove(roomname, out var room))
            {
                return Rooms.Remove(room);
            }
            return false;
        }


        /// <summary>
        /// Elimina todas las salas del panel.
        /// </summary>
        public void ClearRooms()
        {
            Rooms.Clear();
        }

        #endregion

        #region Apoyo
        // Invoca el evento de sala seleccionada:
        partial void OnSelectedRoomChanged(ChatRoomViewModel? value)
        {
            RoomSelected?.Invoke(value);
        }
        #endregion
    }
}
