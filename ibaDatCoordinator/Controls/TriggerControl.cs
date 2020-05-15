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
        [System.Reflection.Obfuscation]
        private class PulseSignal : INotifyPropertyChanged
        {

            string pulseID;

            public event PropertyChangedEventHandler PropertyChanged;

            public string PulseID {
                get { return pulseID; }
                set
                {
                    pulseID = value;
                    RaisePropertyChanged("pulseID");
                }

            }

            public PulseSignal()
                : this("")
            { }

            public PulseSignal(string id)
            {
                PulseID = id ?? "";
            }

            void RaisePropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        BindingList<PulseSignal> pulseSignals;


        public TriggerControl()
        {
            InitializeComponent();
            m_colPulse.OptionsColumn.AllowEdit = true;
            m_colPulse.OptionsColumn.ReadOnly = false;

            pulseSignals = new BindingList<PulseSignal>();
            GrPulse.BeginUpdate();
            try
            {
                GrPulse.DataSource = pulseSignals;
            }
            finally
            {
                GrPulse.EndUpdate();
            }
            pulseSignals.ListChanged += TriggerChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private bool ignoreChanges = false;

        private void m_rbTriggerBySignal_CheckedChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(TriggerBySignal));
        }

        public void SetEnabled(int selectedEventCount)
        {
        }

        void RaisePropertyChanged(string propertyName)
        {
            if (!ignoreChanges)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void TriggerChanged(object sender, EventArgs eventArgs)
        {
            TriggerBySignal = true;
            RaisePropertyChanged("Trigger");
        }

        public void LoadSignal(LocalEventData localEventData)
        {
            ignoreChanges = true;
            HDCreateEventTaskData.EventData m_data = localEventData.Tag as HDCreateEventTaskData.EventData;

            pulseSignals.Clear();
            if (m_data != null)
            {
                pulseSignals.Add(new PulseSignal(m_data.PulseSignal));

                if (m_data.TriggerMode == HDCreateEventTaskData.HDEventTriggerEnum.PerFile)
                    TriggerPerFile = true;
                else
                    TriggerBySignal = true;
            }
            else
            {
                pulseSignals.Add(new PulseSignal(HDCreateEventTaskData.UnassignedExpression));

                TriggerPerFile = true;
            }
            ignoreChanges = false;
        }

        public LocalEventData StoreSignal(LocalEventData localEventData)
        {
            HDCreateEventTaskData.EventData m_data = localEventData.Tag as HDCreateEventTaskData.EventData;

            if (m_data == null)
                m_data = new HDCreateEventTaskData.EventData();

            BindingList<PulseSignal> signals = ((BindingList<PulseSignal>)GrPulse.DataSource);
            if (signals != null && signals.Count > 0)
                m_data.PulseSignal = signals[0].PulseID;

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
            channelEditor.EditValueChanged += TriggerChanged;
            GrPulse.DataSourceChanged += TriggerChanged;

            if (signalDragDropHandler != null)
            {
                OnDragOverHandler -= signalDragDropHandler.OnDragOverHandle;
                OnDragDropHandler -= signalDragDropHandler.OnDragDropHandle;
                signalDragDropHandler.OnDragOverChannelResponse -= OnDragoverResponse;
                signalDragDropHandler.OnDragDropChannelResponse -= OnDragDropResponse;
            }

            signalDragDropHandler = dragDropHandler;
            if (dragDropHandler != null)
            {
                OnDragOverHandler += dragDropHandler.OnDragOverHandle;
                OnDragDropHandler += dragDropHandler.OnDragDropHandle;
                dragDropHandler.OnDragOverChannelResponse += OnDragoverResponse;
                dragDropHandler.OnDragDropChannelResponse += OnDragDropResponse;
            }
        }

        #region Drag/Drop

        public event EventHandler OnDragOverHandler;
        public event EventHandler OnDragDropHandler;

        private ISignalDragDropHandler signalDragDropHandler;

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            OnDragOverHandler?.Invoke(this, new DragDetailsEvent(false, drgevent));
        }

        public void OnDragoverResponse(object sender, EventArgs eventArgs)
        {
            DragDetailsResponseEvent response = eventArgs as DragDetailsResponseEvent;
            if (response != null && response.allowed && sender == signalDragDropHandler && response.signals.Count == 1)
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
                if (sender == signalDragDropHandler && response.signals.Count == 1)
                {
                    GrPulse.BeginUpdate();
                    try
                    {
                        pulseSignals.Clear();
                        pulseSignals.Add(new PulseSignal(response.signals[0].Item1));
                    }
                    finally
                    {
                        GrPulse.EndUpdate();
                    }
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
