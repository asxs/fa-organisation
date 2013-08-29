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

    /// <summary>
    /// 
    /// </summary>
    public class TypedTransactionList : IList<IDbCommand>
    {
        /// <summary>
        /// The commands
        /// </summary>
        protected IDbCommand[] commands = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedTransactionList"/> class.
        /// </summary>
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
        /// <summary>
        /// Ruft das Element am angegebenen Index ab oder legt dieses fest.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// commands
        /// or
        /// value
        /// or
        /// commands
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// index
        /// or
        /// index
        /// </exception>
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
        /// <summary>
        /// Bestimmt den Index eines bestimmten Elements in der <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">Das im <see cref="T:System.Collections.Generic.IList`1" /> zu suchende Objekt.</param>
        /// <returns>
        /// Der Index von <paramref name="item" />, wenn das Element in der Liste gefunden wird, andernfalls -1.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">item</exception>
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
        /// <summary>
        /// Fügt am angegebenen Index ein Element in die <see cref="T:System.Collections.Generic.IList`1" /> ein.
        /// </summary>
        /// <param name="index">Der nullbasierte Index, an dem <paramref name="item" /> eingefügt werden soll.</param>
        /// <param name="item">Das in die <see cref="T:System.Collections.Generic.IList`1" /> einzufügende Objekt.</param>
        /// <exception cref="System.ArgumentNullException">
        /// item
        /// or
        /// item.Connection
        /// </exception>
        /// <exception cref="System.InvalidOperationException">connection must be open</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index</exception>
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
        /// <summary>
        /// Entfernt das <see cref="T:System.Collections.Generic.IList`1" />-Element am angegebenen Index.
        /// </summary>
        /// <param name="index">Der nullbasierte Index des zu entfernenden Elements.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index</exception>
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

        /// <summary>
        /// Ruft die Anzahl der Elemente ab, die in <see cref="T:System.Collections.Generic.ICollection`1" /> enthalten sind.
        /// </summary>
        /// <returns>Die Anzahl der Elemente, die in <see cref="T:System.Collections.Generic.ICollection`1" /> enthalten sind.</returns>
        int Count { get; }
        //
        // Zusammenfassung:
        //     Ruft einen Wert ab, der angibt, ob System.Collections.Generic.ICollection<T>
        //     schreibgeschützt ist.
        //
        // Rückgabewerte:
        //     true, wenn System.Collections.Generic.ICollection<T> schreibgeschützt ist,
        //     andernfalls false.
        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob <see cref="T:System.Collections.Generic.ICollection`1" /> schreibgeschützt ist.
        /// </summary>
        /// <returns>true, wenn <see cref="T:System.Collections.Generic.ICollection`1" /> schreibgeschützt ist, andernfalls false.</returns>
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
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Add(T item);

        //
        // Zusammenfassung:
        //     Entfernt alle Elemente aus System.Collections.Generic.ICollection<T>.
        //
        // Ausnahmen:
        //   System.NotSupportedException:
        //     System.Collections.Generic.ICollection<T> ist schreibgeschützt.
        /// <summary>
        /// Entfernt alle Elemente aus <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
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
        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
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
        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
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
        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        bool Remove(T item);


        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IDataLayer
    {
        /// <summary>
        /// Opens this instance.
        /// </summary>
        void Open();
        /// <summary>
        /// Opens the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        void Open(string connectionString);
        /// <summary>
        /// Tries the open.
        /// </summary>
        /// <returns></returns>
        bool TryOpen();
        /// <summary>
        /// Tries the open.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        bool TryOpen(string connectionString);
        /// <summary>
        /// Opens the with transaction.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        void OpenWithTransaction(IsolationLevel isolationLevel);
        /// <summary>
        /// Updates the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="makeNewConnection">if set to <c>true</c> [make new connection].</param>
        void Update(string connectionString, bool makeNewConnection);
        /// <summary>
        /// Gets the connection information.
        /// </summary>
        void GetConnectionInformation();
        /// <summary>
        /// Closes this instance.
        /// </summary>
        void Close();
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IBasicLayer
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }
        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        IDbCommand Command { get; }
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        IDbConnection Connection { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class DataLayerProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataLayerProperties"/> class.
        /// </summary>
        public DataLayerProperties()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLayerProperties"/> class.
        /// </summary>
        /// <param name="typeOfEditing">The type of editing.</param>
        public DataLayerProperties(StatementType typeOfEditing)
        {

        }

        /// <summary>
        /// Adds the specified type of editing.
        /// </summary>
        /// <param name="typeOfEditing">The type of editing.</param>
        /// <returns></returns>
        protected virtual IDbCommand Add(StatementType typeOfEditing)
        {
            return default(IDbCommand);
        }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        /// <returns></returns>
        protected virtual IDbConnection Open()
        {
            return default(IDbConnection);
        }

        /// <summary>
        /// Opens the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        protected virtual IDbConnection Open(string connectionString)
        {
            return default(IDbConnection);
        }

        /// <summary>
        /// Gets the select command.
        /// </summary>
        /// <value>
        /// The select command.
        /// </value>
        public IDbCommand SelectCommand { get; }
        /// <summary>
        /// Gets the update command.
        /// </summary>
        /// <value>
        /// The update command.
        /// </value>
        public IDbCommand UpdateCommand { get; }
        /// <summary>
        /// Gets the insert command.
        /// </summary>
        /// <value>
        /// The insert command.
        /// </value>
        public IDbCommand InsertCommand { get; }
        /// <summary>
        /// Gets or sets a value indicating whether [use transaction].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use transaction]; otherwise, <c>false</c>.
        /// </value>
        public bool UseTransaction { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class IxSAppDatabaseLayer : IDataLayer
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public enum SqlSystem : int
    {
        /// <summary>
        /// The SQL server
        /// </summary>
        SqlServer = 0,
        /// <summary>
        /// The oracle database
        /// </summary>
        OracleDatabase = 1,
        /// <summary>
        /// The sybase
        /// </summary>
        Sybase = 2,
        /// <summary>
        /// The none
        /// </summary>
        None
    }

    /// <summary>
    /// 
    /// </summary>
    public enum TransactionSystemDirectionType : int
    {
        /// <summary>
        /// The forward
        /// </summary>
        Forward,
        /// <summary>
        /// The backward
        /// </summary>
        Backward,
        /// <summary>
        /// The error
        /// </summary>
        Error
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IxSAppTransactionSystem
    {
        /// <summary>
        /// Sets the isolation level.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        void SetIsolationLevel(IsolationLevel isolationLevel);
        /// <summary>
        /// Resets this instance.
        /// </summary>
        void Reset();
        /// <summary>
        /// Catches the transaction keyword.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        TransactionSystemDirectionType CatchTransactionKeyword(string item);
        /// <summary>
        /// Verifies the keywords.
        /// </summary>
        void VerifyKeywords(); //regexp
        /// <summary>
        /// Generates the command text.
        /// </summary>
        /// <returns></returns>
        string GenerateCommandText();
        /// <summary>
        /// Gets the isolation.
        /// </summary>
        /// <value>
        /// The isolation.
        /// </value>
        IsolationLevel Isolation { get; }
        /// <summary>
        /// Gets the system.
        /// </summary>
        /// <value>
        /// The system.
        /// </value>
        SqlSystem System { get; }
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        TransactionLayerProperties Properties { get; }
        /// <summary>
        /// Gets the keywords.
        /// </summary>
        /// <value>
        /// The keywords.
        /// </value>
        string[] Keywords { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SqlServerTransactionSystem 
        : IxSAppTransactionSystem
    {
        /// <summary>
        /// The transaction string
        /// </summary>
        protected string transactionString = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerTransactionSystem"/> class.
        /// </summary>
        public SqlServerTransactionSystem()
        {

        }

        #region SqlServerTransactionSystem (IxSAppTransactionSystem)

        /// <summary>
        /// Sets the isolation level.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        public void SetIsolationLevel(IsolationLevel isolationLevel)
        {
            Isolation = isolationLevel;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            transactionString = string.Empty;
        }

        /// <summary>
        /// Catches the transaction keyword.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public TransactionSystemDirectionType CatchTransactionKeyword(string item)
        {
            var direction 
                = TransactionSystemDirectionType.Error;

            if (Isolation != IsolationLevel.Chaos && Isolation != IsolationLevel.Unspecified)
            {
                switch (Isolation)
                {
                    case IsolationLevel.ReadCommitted:
                        {
                            item = item.ToUpper();
                            switch (item)
                            {
                                case "BEGIN":
                                    direction = TransactionSystemDirectionType.Forward;
                                    item =
                                        string.Concat(item, " ");
                                    break;
                                case "TRANSACTION":
                                    direction = TransactionSystemDirectionType.Forward;
                                    item =
                                        string.Concat(item, " ");
                                    break;
                                case "SET":
                                    direction = TransactionSystemDirectionType.Forward;
                                    item =
                                        string.Concat(item, " ");
                                    break;
                                case "ISOLATION":
                                    direction = TransactionSystemDirectionType.Forward;
                                    item =
                                        string.Concat(item, " ");
                                    break;
                                case "LEVEL":
                                    direction = TransactionSystemDirectionType.Forward;
                                    item =
                                        string.Concat(item, " ");
                                    break;
                            }
                        }
                        break;
                }
            }

            switch (Isolation)
            {
                case IsolationLevel.ReadCommitted:
                    {
                        item = item.ToUpper();
                        switch (item)
                        {
                            case "READ":
                                direction = TransactionSystemDirectionType.Forward;
                                item =
                                    string.Concat(item, " ");
                                break;
                            case "COMMITTED":
                                direction = TransactionSystemDirectionType.Forward;
                                item =
                                    string.Concat(item, " ");
                                break;
                        }
                    }
                    break;
            }

            if (direction ==
                TransactionSystemDirectionType.Forward)
                transactionString = string.Concat(transactionString, item);

            return direction;
        }

        /// <summary>
        /// Verifies the keywords.
        /// </summary>
        public void VerifyKeywords() //regexp
        {

        }

        /// <summary>
        /// Generates the command text.
        /// </summary>
        /// <returns></returns>
        public string GenerateCommandText()
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the isolation.
        /// </summary>
        /// <value>
        /// The isolation.
        /// </value>
        public IsolationLevel Isolation { get; internal set; }
        /// <summary>
        /// Gets the system.
        /// </summary>
        /// <value>
        /// The system.
        /// </value>
        public SqlSystem System { get; internal set; }
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public TransactionLayerProperties Properties { get; internal set; }
        /// <summary>
        /// Gets the keywords.
        /// </summary>
        /// <value>
        /// The keywords.
        /// </value>
        public string[] Keywords { get; internal set; } 

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public struct TransactionLayerProperties
    {
        /// <summary>
        /// Gets the command text.
        /// </summary>
        /// <value>
        /// The command text.
        /// </value>
        public string CommandText
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors 
        { 
            get; 
            internal set; 
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public IDbCommand Command { get; internal set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class IxSAppCommandFactory
    {
        /// <summary>
        /// Creates the qualified transaction command.
        /// </summary>
        /// <param name="system">The system.</param>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException"></exception>
        public static string CreateQualifiedTransactionCommand(SqlSystem system, IsolationLevel isolationLevel)
        {
            IxSAppTransactionSystem
                transactionSystem = null;

            switch (system)
            {
                case SqlSystem.Sybase:
                    {

                    }
                    break;
                case SqlSystem.SqlServer:
                    {
                        transactionSystem = new SqlServerTransactionSystem();
                    }
                    break;
                case SqlSystem.OracleDatabase:
                    {

                    }
                    break;
            }

            transactionSystem.SetIsolationLevel(isolationLevel: isolationLevel);

            foreach (var keyword in
                new string[] { "BEGIN", "TRANSACTION", "SET", "ISOLATION", "LEVEL", "READ", "COMMITTED", "UNCOMMITTED", "SERIALZE", "SNAPSHOT" })
            {
                var direction 
                    = transactionSystem.CatchTransactionKeyword(item: keyword);

                if (direction == TransactionSystemDirectionType.Forward) //its really unnecessary... (dummy for future)
                    continue;
                else
                    if (direction ==
                        TransactionSystemDirectionType.Backward)
                        throw new FormatException(keyword);
            }

            var commandText = 
                transactionSystem.GenerateCommandText();
            
            return commandText;
        }
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
        /// Initializes a new instance of the <see cref="FaOrganisationAbstract" /> class.
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
        /// <exception cref="System.InvalidOperationException">connection have to be initialized
        /// or
        /// connection have to be open</exception>
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

        /// <summary>
        /// Opens the transaction internal.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">command</exception>
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

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="transactionCommand">The transaction command.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">transactionCommand</exception>
        /// <exception cref="System.InvalidOperationException">no open connection</exception>
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

        /// <summary>
        /// Commits all transactions.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates the command internal.
        /// </summary>
        /// <param name="createNewCommand">if set to <c>true</c> [create new command].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">connection must have a value</exception>
        /// <exception cref="System.Data.DataException">connection must be open before it can be used</exception>
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
