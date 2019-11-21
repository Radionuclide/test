using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using iba.HD.Client.Interfaces;
using iba.Data;

namespace iba.Controls
{
    public partial class TriggerControl : UserControl, IEventTrigger, INotifyPropertyChanged
    {
        private class PulseSignal
        {
            public string PulseID { get; set; }

            public PulseSignal()
                : this("")
            { }

            public PulseSignal(string id)
            {
                PulseID = id ?? "";
            }
        }

        public TriggerControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void m_rbTriggerBySignal_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbTriggerBySignal.Checked)
            {
                m_colPulse.OptionsColumn.AllowEdit = true;
                m_colPulse.OptionsColumn.ReadOnly = false;
            }
            else
            {
                m_colPulse.OptionsColumn.AllowEdit = false;
                m_colPulse.OptionsColumn.ReadOnly = true;
            }
            RaisePropertyChanged(nameof(TriggerBySignal));
        }

        public void SetEnabled(int selectedEventCount)
        {
        }

        void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoadSignal(LocalEventData localEventData)
        {
            HDCreateEventTaskData.EventData m_data = localEventData.Tag as HDCreateEventTaskData.EventData;
            if (m_data != null)
            {
                GrPulse.DataSource = new List<PulseSignal>(1) { new PulseSignal(m_data.PulseSignal) };
                if (m_data.TriggerMode == HDCreateEventTaskData.HDEventTriggerEnum.PerFile)
                    TriggerPerFile = true;
                else
                    TriggerBySignal = true;
            }
            else
            {
                GrPulse.DataSource = new List<PulseSignal>(1) { new PulseSignal(HDCreateEventTaskData.UnassignedExpression) };
                TriggerPerFile = true;
            }
        }

        public LocalEventData StoreSignal(LocalEventData localEventData)
        {
            HDCreateEventTaskData.EventData m_data = localEventData.Tag as HDCreateEventTaskData.EventData;
            if (m_data == null)
                m_data = new HDCreateEventTaskData.EventData();
            List<PulseSignal> signals = ((List<PulseSignal>)GrPulse.DataSource);
            if (signals != null && signals.Count == 1)
                m_data.PulseSignal = ((List<PulseSignal>)GrPulse.DataSource)[0].PulseID;
            if (TriggerPerFile)
                m_data.TriggerMode = HDCreateEventTaskData.HDEventTriggerEnum.PerFile;
            else
                m_data.TriggerMode = HDCreateEventTaskData.HDEventTriggerEnum.PerSignalPulse;
            localEventData.Tag = m_data;

            return localEventData;
        }

        public bool TriggerPerFile
        {
            get { return m_rbTriggerPerFile.Checked; }
            set { m_rbTriggerPerFile.Checked = value; }
        }

        public bool TriggerBySignal
        {
            get { return m_rbTriggerBySignal.Checked; }
            set { m_rbTriggerBySignal.Checked = value; }
        }

        public GridControl GrPulse
        {
            get { return m_grPulse; }
        }

        public GridColumn ColPulse
        {
            get { return m_colPulse; }
        }

        public void SetChannelEditor(DevExpress.XtraEditors.Repository.RepositoryItem channelEditor, ISignalDragDropHandler dragDropHandler)
        {
            GrPulse.RepositoryItems.Add(channelEditor);
            ColPulse.ColumnEdit = channelEditor;
            if (signalDragDropHandler != null)
            {
                OnDragOverHandler -= signalDragDropHandler.OnDragOverHandle;
                OnDragDropHandler -= signalDragDropHandler.OnDragDropHandle;
                handlerDrag = signalDragDropHandler.OnDragOverChannelResponse;
                handlerDrag -= OnDragoverResponse;
                signalDragDropHandler.OnDragOverChannelResponse = handlerDrag;
                HandlerDrop -= OnDragDropResponse;
                signalDragDropHandler.OnDragDropChannelResponse = HandlerDrop;
            }
            signalDragDropHandler = dragDropHandler;
            if (dragDropHandler != null)
            {
                OnDragOverHandler += dragDropHandler.OnDragOverHandle;
                OnDragDropHandler += dragDropHandler.OnDragDropHandle;
                handlerDrag = dragDropHandler.OnDragOverChannelResponse;
                handlerDrag += OnDragoverResponse;
                dragDropHandler.OnDragOverChannelResponse = handlerDrag;
                HandlerDrop += OnDragDropResponse;
                dragDropHandler.OnDragDropChannelResponse = HandlerDrop;
            }
        }

        #region Drag/Drop

        public event EventHandler OnDragOverHandler;
        public event EventHandler OnDragDropHandler;

        private event EventHandler handlerDrag;
        public event EventHandler HandlerDrop;
        private ISignalDragDropHandler signalDragDropHandler;

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            OnDragOverHandler?.Invoke(this, new DragDetailsEvent(false, drgevent));
        }

        public void OnDragoverResponse(object sender, EventArgs eventArgs)
        {
            DragDetailsResponseEvent response = eventArgs as DragDetailsResponseEvent;
            if (response != null && response.allowed && sender == signalDragDropHandler)
                response.drgEvent.Effect = response.drgEvent.AllowedEffect;
            else
                response.drgEvent.Effect = DragDropEffects.None;
            base.OnDragOver(response.drgEvent);
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            OnDragDropHandler?.Invoke(this, new DragDetailsEvent(false, drgevent));
        }

        public void OnDragDropResponse(object sender, EventArgs eventArgs)
        {
            if (eventArgs is DragDetailsResponseEvent response && response.allowed)
            {
                if (sender == signalDragDropHandler)
                {
                    GrPulse.DataSource = new List<PulseSignal>(1) { new PulseSignal(response.id) };
                }
            }
        }


        public override bool AllowDrop
        {
            get { return true; }
        }
        #endregion
    }
}
