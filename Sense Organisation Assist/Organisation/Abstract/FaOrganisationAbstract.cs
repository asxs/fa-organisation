/* 
 * Builded by Lars Ulrich Herrmann (c) 2013 with f.fN. Sense Applications in year August 2013
 * The code can be used in other Applications if it makes sense
 * If it makes sense the code can be used in this Application
 * I hold the rights on all lines of code and if it makes sense you can contact me over the publish site
 * Feel free to leave a comment
 * 
 * Good Bye
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;

#region iAnywhere.Data.SQLAnywhere.v3.5

using iAnywhere;
using iAnywhere.Data;
using iAnywhere.Data.SQLAnywhere;

#endregion

namespace IxSApp
{
    using Data;

    public class TypedTransactionList : IList<IDbCommand>
    {
        protected IDbCommand[] commands = null;

        public TypedTransactionList()
            : base()
        {
            commands = new IDbCommand[] { };
        }

        #region TransactionList (IList<IDbCommand>)

        // Zusammenfassung:
        //     Ruft das Element am angegebenen Index ab oder legt dieses fest.
        //
        // Parameter:
        //   index:
        //     Der nullbasierte Index des Elements, das abgerufen oder festgelegt werden
        //     soll.
        //
        // Rückgabewerte:
        //     Das Element am angegebenen Index.
        //
        // Ausnahmen:
        //   System.ArgumentOutOfRangeException:
        //     index ist kein gültiger Index in der System.Collections.Generic.IList<T>.
        //
        //   System.NotSupportedException:
        //     Die Eigenschaft wird festgelegt, und die System.Collections.Generic.IList<T>
        //     ist schreibgeschützt.
        public IDbCommand this[int index]
        {
            get
            {
                if (commands == null)
                    throw new ArgumentNullException("commands");

                if (commands.Length == 0 || index > commands.Length || index == -1 || index < 0)
                    throw new ArgumentOutOfRangeException("index");

                return commands[index];
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (commands == null)
                    throw new ArgumentNullException("commands");

                if (commands.Length == 0 || index > commands.Length || index == -1 || index < 0)
                    throw new ArgumentOutOfRangeException("index");

                commands[index]
                    = value;
            }
        }

        // Zusammenfassung:
        //     Bestimmt den Index eines bestimmten Elements in der System.Collections.Generic.IList<T>.
        //
        // Parameter:
        //   item:
        //     Das im System.Collections.Generic.IList<T> zu suchende Objekt.
        //
        // Rückgabewerte:
        //     Der Index von item, wenn das Element in der Liste gefunden wird, andernfalls
        //     -1.
        public int IndexOf(IDbCommand item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return Array.FindIndex<IDbCommand>(commands, (x) => x.Equals(item));
        }

        //
        // Zusammenfassung:
        //     Fügt am angegebenen Index ein Element in die System.Collections.Generic.IList<T>
        //     ein.
        //
        // Parameter:
        //   index:
        //     Der nullbasierte Index, an dem item eingefügt werden soll.
        //
        //   item:
        //     Das in die System.Collections.Generic.IList<T> einzufügende Objekt.
        //
        // Ausnahmen:
        //   System.ArgumentOutOfRangeException:
        //     index ist kein gültiger Index in der System.Collections.Generic.IList<T>.
        //
        //   System.NotSupportedException:
        //     Die System.Collections.Generic.IList<T> ist schreibgeschützt.
        public void Insert(int index, IDbCommand item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (item.Connection == null)
                throw new ArgumentNullException("item.Connection");

            if (item.Connection.State != ConnectionState.Open)
                throw new InvalidOperationException("connection must be open");

            if (index == -1 || index < -1)
                throw new ArgumentOutOfRangeException("index");

            commands
                = commands.Add<IDbCommand>(item);
        }

        //
        // Zusammenfassung:
        //     Entfernt das System.Collections.Generic.IList<T>-Element am angegebenen Index.
        //
        // Parameter:
        //   index:
        //     Der nullbasierte Index des zu entfernenden Elements.
        //
        // Ausnahmen:
        //   System.ArgumentOutOfRangeException:
        //     index ist kein gültiger Index in der System.Collections.Generic.IList<T>.
        //
        //   System.NotSupportedException:
        //     Die System.Collections.Generic.IList<T> ist schreibgeschützt.
        public void RemoveAt(int index)
        {
            if (index == -1 || index < -1 || index > commands.Length)
                throw new ArgumentOutOfRangeException("index");

            commands
                = commands.RemoveAt<IDbCommand>(index);
        }

        #endregion

        #region TransactionList (ICollection<IDbCommand>)

        // Zusammenfassung:
        //     Ruft die Anzahl der Elemente ab, die in System.Collections.Generic.ICollection<T>
        //     enthalten sind.
        //
        // Rückgabewerte:
        //     Die Anzahl der Elemente, die in System.Collections.Generic.ICollection<T>
        //     enthalten sind.

        int Count { get; }
        //
        // Zusammenfassung:
        //     Ruft einen Wert ab, der angibt, ob System.Collections.Generic.ICollection<T>
        //     schreibgeschützt ist.
        //
        // Rückgabewerte:
        //     true, wenn System.Collections.Generic.ICollection<T> schreibgeschützt ist,
        //     andernfalls false.
        bool IsReadOnly { get; }

        // Zusammenfassung:
        //     Fügt der System.Collections.Generic.ICollection<T> ein Element hinzu.
        //
        // Parameter:
        //   item:
        //     Das Objekt, das System.Collections.Generic.ICollection<T> hinzugefügt werden
        //     soll.
        //
        // Ausnahmen:
        //   System.NotSupportedException:
        //     System.Collections.Generic.ICollection<T> ist schreibgeschützt.
        void Add(T item);

        //
        // Zusammenfassung:
        //     Entfernt alle Elemente aus System.Collections.Generic.ICollection<T>.
        //
        // Ausnahmen:
        //   System.NotSupportedException:
        //     System.Collections.Generic.ICollection<T> ist schreibgeschützt.
        void Clear();

        //
        // Zusammenfassung:
        //     Bestimmt, ob System.Collections.Generic.ICollection<T> einen bestimmten Wert
        //     enthält.
        //
        // Parameter:
        //   item:
        //     Das im System.Collections.Generic.ICollection<T> zu suchende Objekt.
        //
        // Rückgabewerte:
        //     true, wenn sich item in System.Collections.Generic.ICollection<T> befindet,
        //     andernfalls false.
        bool Contains(T item);

        //
        // Zusammenfassung:
        //     Kopiert die Elemente von System.Collections.Generic.ICollection<T> in ein
        //     System.Array, beginnend bei einem bestimmten System.Array-Index.
        //
        // Parameter:
        //   array:
        //     Das eindimensionale System.Array, das das Ziel der aus System.Collections.Generic.ICollection<T>
        //     kopierten Elemente ist. Für System.Array muss eine nullbasierte Indizierung
        //     verwendet werden.
        //
        //   arrayIndex:
        //     Der nullbasierte Index in array, ab dem kopiert wird.
        //
        // Ausnahmen:
        //   System.ArgumentNullException:
        //     array ist null.
        //
        //   System.ArgumentOutOfRangeException:
        //     arrayIndex ist kleiner als 0 (null).
        //
        //   System.ArgumentException:
        //     array ist mehrdimensional.– oder –arrayIndex ist größer oder gleich der Länge
        //     von array.– oder –Die Anzahl der aus der Quell-System.Collections.Generic.ICollection<T>
        //     zu kopierenden Elemente ist größer als der verfügbare Platz von arrayIndex
        //     bis zum Ende des Ziel-array.– oder –Typ T kann nicht automatisch in den Typ
        //     des Ziel-array umgewandelt werden.
        void CopyTo(T[] array, int arrayIndex);

        //
        // Zusammenfassung:
        //     Entfernt das erste Vorkommen eines bestimmten Objekts aus System.Collections.Generic.ICollection<T>.
        //
        // Parameter:
        //   item:
        //     Das aus dem System.Collections.Generic.ICollection<T> zu entfernende Objekt.
        //
        // Rückgabewerte:
        //     true, wenn item erfolgreich aus System.Collections.Generic.ICollection<T>
        //     gelöscht wurde, andernfalls false. Diese Methode gibt auch dann false zurück,
        //     wenn item nicht in der ursprünglichen System.Collections.Generic.ICollection<T>
        //     gefunden wurde.
        //
        // Ausnahmen:
        //   System.NotSupportedException:
        //     System.Collections.Generic.ICollection<T> ist schreibgeschützt.
        bool Remove(T item);


        #endregion
    }

    public interface IDataLayer
    {
        void Open();
        void Open(string connectionString);
        void TryOpen();
        void TryOpen(string connectionString);
        void OpenWithTransaction(IsolationLevel isolationLevel);
        void Update(string connectionString, bool makeNewConnection);
        void GetConnectionInformation();
        void Close();
    }

    public interface IBasicLayer 
    {
        string Name { get; }
        IDbCommand Command { get; }
        IDbConnection Connection { get; }
    }

    public sealed class DataLayerProperties
    {
        public DataLayerProperties()
        {

        }

        public DataLayerProperties(StatementType typeOfEditing)
        {

        }

        protected virtual IDbCommand Add(StatementType typeOfEditing)
        {
            return default(IDbCommand);
        }

        protected virtual IDbConnection Open()
        {
            return default(IDbConnection);
        }

        protected virtual IDbConnection Open(string connectionString)
        {
            return default(IDbConnection);
        }

        public IDbCommand SelectCommand { get; }
        public IDbCommand UpdateCommand { get; }
        public IDbCommand InsertCommand { get; }
        public bool UseTransaction { get; set; }
    }

    public sealed class IxSAppDatabaseLayer : IDataLayer
    {
    
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class FaOrganisationAbstract
        : IFaOrganisation, IDisposable
    {
        /// <summary>
        /// The connection
        /// </summary>
        protected SAConnection connection = null;
        /// <summary>
        /// The table id
        /// </summary>
        protected TableIdCommand tableId = null;

        /// <summary>
        /// The command
        /// </summary>
        protected SACommand command = null;

        ///// <summary>
        ///// The transactions
        ///// </summary>
        //protected TypedTransactionList transactions = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FaOrganisationAbstract"/> class.
        /// </summary>
        public FaOrganisationAbstract()
        {
            //transactions = new TypedTransactionList();
        }

        #region FaOrganisationAppend (IDisposable)

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben durch, die mit der Freigabe, der Zurückgabe oder dem Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                connection = null;
                tableId = null;
            }
        }

        #endregion

        /// <summary>
        /// Opens the internal.
        /// </summary>
        /// <param name="encapsulateWithTransaction">if set to <c>true</c> [encapsulate with transaction].</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">
        /// connection have to be initialized
        /// or
        /// connection have to be open
        /// </exception>
        protected virtual IDbConnection OpenInternalWithTransaction(bool encapsulateWithTransaction)
        {
            if (connection == null)
            {
                try
                {
                    connection =
                        ConnectionFactory.CreateAnySystemConnection(ConnectionStringManager.ConnectionStringNetworkServer) as SAConnection;

                    connection.Open();

                    if (connection == null)
                        throw new InvalidOperationException("connection have to be initialized");

                    if (connection.State != ConnectionState.Open)
                        throw new InvalidOperationException("connection have to be open");

                    if (encapsulateWithTransaction)
                    {
                        try
                        {
                            //OpenTransactionInternal(isolationLevel: IsolationLevel.ReadCommitted);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return connection;
        }

        protected virtual bool OpenTransactionInternal(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var result 
                = true;

            command = CreateCommandInternal() as SACommand;
            if (command == null)
                throw new ArgumentNullException("command");

            try
            {
                command.CommandText = "begin transaction";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }

            return result;
        }

        protected virtual bool CommitTransaction(IDbCommand transactionCommand)
        {
            var result = true;

            if (transactionCommand == null)
                throw new ArgumentNullException("transactionCommand");

            if (transactionCommand.Connection.State != ConnectionState.Open)
                throw new InvalidOperationException("no open connection");

            try
            {
                transactionCommand.CommandText = "commit";
                transactionCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }

            return result;
        }

        protected virtual bool CommitAllTransactions(params IDbCommand[] commands)
        {
            var result = true;

            foreach (var command in commands)
            {
                try
                {
                    CommitTransaction(transactionCommand: command);
                }
                catch (Exception ex)
                {
                    result = false;
                    throw ex;
                }
            }

            return result;
        }

        protected virtual IDbCommand CreateCommandInternal(bool createNewCommand = false)
        {
            if (connection == null)
                throw new ArgumentException("connection must have a value");

            if (connection.State != ConnectionState.Open)
                throw new DataException("connection must be open before it can be used");

            if (command != null || createNewCommand)
                command = connection.CreateCommand();

            return command;
        }

        /// <summary>
        /// Appends the specified package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="unit">The unit.</param>
        /// <param name="saveWithNewId">if set to <c>true</c> [save with new id].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">unit</exception>
        /// <exception cref="System.InvalidOperationException">tableIdCommand have to be initialized</exception>
        public virtual long Append(Units package, IWorkDataUnit unit, bool saveWithNewId = true)
        {
            var key = 0L;

            if (unit == null)
                throw new ArgumentNullException("unit");

            OpenInternalWithTransaction(encapsulateWithTransaction: true);

            using (var command =
                connection.CreateCommand())
            {
                var tableId = 
                    TableIdCommand.CreateInstance(command as IDbCommand);

                if (tableId == null)
                    throw new InvalidOperationException("tableIdCommand have to be initialized");

                key =
                    tableId.Get(saveWithNewId ? "NEW" : string.Empty, unit.Table);

                unit.Command = command;
                unit.Database = "asxs";
                unit.Priority = unit.Table.ToUpper().Contains("firm") ? UnitPriorityType.Head : UnitPriorityType.Children;
                unit.Insert(package, key);
            }

            return key;
        }

        /// <summary>
        /// Edits the specified package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="unit">The unit.</param>
        /// <param name="id">The id.</param>
        /// <exception cref="System.ArgumentNullException">unit</exception>
        /// <exception cref="System.InvalidOperationException">tableIdCommand have to be initialized</exception>
        public virtual void Edit(Units package, IWorkDataUnit unit, long id = 0)
        {
            var key = id;

            if (unit == null)
                throw new ArgumentNullException("unit");

            OpenInternalWithTransaction(encapsulateWithTransaction: true);

            using (var command =
                connection.CreateCommand())
            {
                tableId =
                    TableIdCommand.CreateInstance(command);

                if (tableId == null)
                    throw new InvalidOperationException("tableIdCommand have to be initialized");

                unit.Command = command;
                unit.Database = "asxs";
                unit.Priority = unit.Table.Contains("firm") ? UnitPriorityType.Head : UnitPriorityType.Children;
                unit.Update(package, key);
            }
        }

        /// <summary>
        /// Rollbacks this instance.
        /// </summary>
        protected virtual void Rollback()
        {
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "rollback";
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
            catch { }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public virtual void Close()
        {
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "commit";
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
            catch { }
        }
    }

}
