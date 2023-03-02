﻿namespace FsSpreadsheet

type DataType = 
    | String
    | Boolean
    | Number
    | Date
    | Empty

    /// Returns the proper CellValues case for the given value.
    static member InferCellValue (value : 'T) = 
        let value = box value
        match value with
        | :? char as c -> DataType.String,c.ToString()
        | :? string as s -> DataType.String,s.ToString()
        | :? bool as c -> DataType.Boolean,c.ToString()
        | :? byte as i -> DataType.Number,i.ToString()
        | :? sbyte as i -> DataType.Number,i.ToString()
        | :? int as i -> DataType.Number,i.ToString()
        | :? int16 as i -> DataType.Number,i.ToString()
        | :? int64 as i -> DataType.Number,i.ToString()
        | :? uint as i -> DataType.Number,i.ToString()
        | :? uint16 as i -> DataType.Number,i.ToString()
        | :? uint64 as i -> DataType.Number,i.ToString()
        | :? single as i -> DataType.Number,i.ToString()
        | :? float as i -> DataType.Number,i.ToString()
        | :? decimal as i -> DataType.Number,i.ToString()
        | :? System.DateTime as d -> DataType.Date,d.ToString()
        | _ ->  DataType.String,value.ToString()

// Type based on the type XLCell used in ClosedXml
/// Creates an FsCell of `DataType` dataType, with value of type `string`, and `FsAddress` address.
type FsCell (value : string, dataType : DataType, address : FsAddress) =
    
    let mutable _cellValue = value
    let mutable _dataType = dataType
    let mutable _comment  = ""
    let mutable _hyperlink = ""
    let mutable _richText = ""
    let mutable _formulaA1 = ""
    let mutable _formulaR1C1 = ""

    let mutable _rowIndex : int = address.RowNumber
    let mutable _columnIndex : int = address.ColumnNumber


    // ------------------------
    // ALTERNATIVE CONSTRUCTORS
    // ------------------------

    /// Creates an empty FsCell, set at row 1, column 1 (1-based).
    new () = FsCell ("", DataType.Empty, FsAddress(0,0))
    /// Creates an FsCell of `DataType` `String`, with the given value, set at row 1, column 1 (1-based).
    new (value : string) = FsCell (value, DataType.String, FsAddress(0,0))
    /// Creates an FsCell of `DataType` `Number`, with the given value, set at row 1, column 1 (1-based).
    new (value : int) = FsCell (string value, DataType.Number, FsAddress(0,0))
    /// Creates an FsCell of `DataType` `Number`, with the given value, set at row 1, column 1 (1-based).
    new (value : float) = FsCell (string value, DataType.Number, FsAddress(0,0))
    /// Creates an empty FsCell, set at `FsAddress` address.
    new (address : FsAddress) =
        FsCell ("", DataType.Empty, address)
    /// Creates an empty FsCell, set at row rowIndex and column colIndex.
    new (rowIndex, colIndex) =
        FsCell("", DataType.Empty, FsAddress(rowIndex, colIndex))


    // ----------
    // PROPERTIES
    // ----------

    member internal self.SharedStringId = raise (System.NotImplementedException())

    member self.Active = raise (System.NotImplementedException())
    
    /// <summary>Gets this FsCell's address, relative to the FsWorksheet.</summary>
    /// <value>The FsCell's address.</value>
    member self.Address 
        with get() = FsAddress(_columnIndex,_rowIndex)
        and internal set(address : FsAddress) =
            _rowIndex <- address.RowNumber
            _columnIndex <- address.ColumnNumber

    /// <summary>
    /// Calculated value of cell formula. Is used for decreasing number of computations perfromed.
    /// May hold invalid value when <see cref="NeedsRecalculation"/> flag is True.
    /// </summary>
    member self.CachedValue = raise (System.NotImplementedException())
    
    /// <summary>
    /// Returns the current region. The current region is a range bounded by any combination of blank rows and blank columns
    /// </summary>
    /// <value>
    /// The current region.
    /// </value>
    member self.CurrentRegion = raise (System.NotImplementedException())
    
    /// <summary>
    /// Gets or sets the DataType of this FsCell's data.
    /// <para>Changing the data type will cause FsSpreadsheet to convert the current value to the new DataType. </para>
    /// <para>An exception will be thrown if the current value cannot be converted to the new DataType.</para>
    /// </summary>
    /// <value>
    /// The type of the FsCell's data.
    /// </value>
    /// <exception cref="ArgumentException"></exception>
    member self.DataType 
        with get() = _dataType
        and internal set(dataType) = _dataType <- dataType
    
    /// <summary>
    /// Gets or sets the cell's formula with A1 references.
    /// </summary>
    /// <value>The formula with A1 references.</value>
    member self.FormulaA1 = raise (System.NotImplementedException())
    
    /// <summary>
    /// Gets or sets the cell's formula with R1C1 references.
    /// </summary>
    /// <value>The formula with R1C1 references.</value>
    member self.FormulaR1C1 = raise (System.NotImplementedException())
    
    member self.FormulaReference = raise (System.NotImplementedException())
    
    member self.HasArrayFormula = raise (System.NotImplementedException())
    
    member self.HasComment = raise (System.NotImplementedException())
    
    member self.HasDataValidation = raise (System.NotImplementedException())
    
    member self.HasFormula = raise (System.NotImplementedException())
    
    member self.HasHyperlink = raise (System.NotImplementedException())
    
    member self.HasRichText = raise (System.NotImplementedException())
    
    member self.HasSparkline = raise (System.NotImplementedException())
    
    /// <summary>
    /// Flag indicating that previously calculated FsCell value may be not valid anymore and has to be re-evaluated.
    /// </summary>
    member self.NeedsRecalculation = raise (System.NotImplementedException())
    
    /// <summary>
    /// Gets or sets a value indicating whether this cell's text should be shared or not.
    /// </summary>
    /// <value>
    ///   If false the cell's text will not be shared and stored as an inline value.
    /// </value>
    member self.ShareString = raise (System.NotImplementedException())
    
    member self.Sparkline = raise (System.NotImplementedException())
    
    /// <summary>
    /// Gets or sets the FsCell's style.
    /// </summary>
    member self.Style = raise (System.NotImplementedException())
    
    /// <summary>
    /// Gets or sets the cell's value. To get or set a strongly typed value, use the GetValue&lt;T&gt; and SetValue methods.
    /// </summary>
    /// <value>
    /// The object containing the value(s) to set.
    /// </value>
    member self.Value 
        with get() = _cellValue
        and set(value) = _cellValue <- value
    
    /// Gets or sets the FsCell's associated FsWorksheet.
    member self.Worksheet = raise (System.NotImplementedException())

    /// Gets or sets the columnIndex of the FsCell.
    member self.WorksheetColumn
        with get() = _columnIndex
        and set(colI) = _columnIndex <- colI
    
    /// Gets or sets the rowIndex of the FsCell.
    member self.WorksheetRow
        with get() = _rowIndex
        and set(rowI) = _rowIndex <- rowI


    // ------------------
    // NON-STATIC METHODS
    // ------------------
    
    member self.AddConditionalFormat()  = raise (System.NotImplementedException())
    
    /// <summary>
    /// Creates a named range out of this cell.
    /// <para>If the named range exists, it will add this range to that named range.</para>
    /// <para>The default scope for the named range is Workbook.</para>
    /// </summary>
    /// <param name="rangeName">Name of the range.</param>
    member self.AddToNamed(rangeName)  = raise (System.NotImplementedException())
    
    /// <summary>
    /// Creates a named range out of this cell.
    /// <para>If the named range exists, it will add this range to that named range.</para>
    /// <param name="rangeName">Name of the range.</param>
    /// <param name="scope">The scope for the named range.</param>
    /// </summary>
    member self.AddToNamed(rangeName, scope) = raise (System.NotImplementedException())
    
    /// <summary>
    /// Creates a named range out of this cell.
    /// <para>If the named range exists, it will add this range to that named range.</para>
    /// <param name="rangeName">Name of the range.</param>
    /// <param name="scope">The scope for the named range.</param>
    /// <param name="comment">The comments for the named range.</param>
    /// </summary>
    member self.AddToNamed(rangeName, scope, comment) = raise (System.NotImplementedException())
    
    /// <summary>
    /// Returns this cell as an IXLRange.
    /// </summary>
    member self.AsRange()  = raise (System.NotImplementedException())
    
    member self.CellAbove() = raise (System.NotImplementedException())
    
    member self.CellAbove(step) = raise (System.NotImplementedException())
    
    member self.CellBelow() = raise (System.NotImplementedException())
    
    member self.CellBelow(step) = raise (System.NotImplementedException())
    
    member self.CellLeft() = raise (System.NotImplementedException())
    
    member self.CellLeft(step) = raise (System.NotImplementedException())
    
    member self.CellRight() = raise (System.NotImplementedException())
    
    member self.CellRight(step) = raise (System.NotImplementedException())
    
    // see https://github.com/ClosedXML/ClosedXML/blob/develop/ClosedXML/Excel/Cells/XLCell.cs#L860
    /// <summary>
    /// Clears the contents of this FsCell.
    /// </summary>
    /// <param name="clearOptions">Specify what you want to clear.</param>
    member self.Clear(clearOptions(* = XLClearOptions.All*)) = raise (System.NotImplementedException())
    
    //member self.CopyFrom(member self.otherCell);
    
    /// Copies and replaces DataType and Value from a given FsCell into this one.
    member self.CopyFrom(otherCell : FsCell) = 
        self.DataType <- otherCell.DataType
        self.Value <- otherCell.Value
    
    //member self.CopyTo(member self.target);
    
    /// Copies DataType and Value from this FsCell to a given one and replaces theirs.
    member self.CopyTo(target : FsCell) = 
        target.DataType <- self.DataType
        target.Value <- self.Value
    
    /// <summary>
    /// Creates a new comment for the cell, replacing the existing one.
    /// </summary>
    member self.CreateComment() = raise (System.NotImplementedException())
    
    /// <summary>
    /// Creates a new data validation rule for the cell, replacing the existing one.
    /// </summary>
    member self.CreateDataValidation() = raise (System.NotImplementedException())
    
    /// <summary>
    /// Creates a new hyperlink replacing the existing one.
    /// </summary>
    member self.CreateHyperlink() = raise (System.NotImplementedException())
    
    /// <summary>
    /// Replaces a value of the cell with a newly created rich text object.
    /// </summary>
    member self.CreateRichText() = raise (System.NotImplementedException())
    
    /// <summary>
    /// Deletes the current cell and shifts the surrounding cells according to the shiftDeleteCells parameter.
    /// </summary>
    /// <param name="shiftDeleteCells">How to shift the surrounding cells.</param>
    member self.Delete(shiftDeleteCells) = raise (System.NotImplementedException())
    
    /// <summary>
    /// Gets the cell's value converted to Boolean.
    /// <para>ClosedXML will try to covert the current value to Boolean.</para>
    /// <para>An exception will be thrown if the current value cannot be converted to Boolean.</para>
    /// </summary>
    member self.GetBoolean() = raise (System.NotImplementedException())
    
    /// <summary>
    /// Returns the comment for the cell or create a new instance if there is no comment on the cell.
    /// </summary>
    member self.GetComment() = raise (System.NotImplementedException())
    
    /// <summary>
    /// Returns a data validation rule assigned to the cell, if any, or creates a new instance of data validation rule if no rule exists.
    /// </summary>
    member self.GetDataValidation() = raise (System.NotImplementedException())
    
    /// <summary>
    /// Gets the cell's value converted to DateTime.
    /// <para>ClosedXML will try to covert the current value to DateTime.</para>
    /// <para>An exception will be thrown if the current value cannot be converted to DateTime.</para>
    /// </summary>
    member self.GetDateTime() = raise (System.NotImplementedException())
    
    /// <summary>
    /// Gets the FsCell's value converted to Double (64-bit float).
    /// <para>FsSpreadsheet will try to convert the current value to Double.</para>
    /// <para>An exception will be thrown if the current value cannot be converted to Double.</para>
    /// </summary>
    member self.GetDouble() = 
        try float self.Value
        with _ -> failwith $"FsCell with DataType {self.DataType} cannot be converted to Double."
    
    /// <summary>
    /// Gets the cell's value formatted depending on the cell's data type and style.
    /// </summary>
    member self.GetFormattedString() = raise (System.NotImplementedException())
    
    /// <summary>
    /// Returns a hyperlink for the cell, if any, or creates a new instance is there is no hyperlink.
    /// </summary>
    member self.GetHyperlink() = raise (System.NotImplementedException())
    
    /// <summary>
    /// Returns the value of the cell if it formatted as a rich text.
    /// </summary>
    member self.GetRichText() = raise (System.NotImplementedException())
    
    /// <summary>
    /// Gets the FsCell's value converted to a String.
    /// </summary>
    member self.GetString() = value
    
    /// <summary>
    /// Gets the cell's value converted to TimeSpan.
    /// <para>ClosedXML will try to covert the current value to TimeSpan.</para>
    /// <para>An exception will be thrown if the current value cannot be converted to TimeSpan.</para>
    /// </summary>
    member self.GetTimeSpan() = raise (System.NotImplementedException())
    
    /// <summary>
    /// Gets the cell's value converted to the T type.
    /// <para>FsSpreadsheet will try to convert the current value to type 'T.</para>
    /// <para>An exception will be thrown if the current value cannot be converted to the T type.</para>
    /// </summary>
    /// <typeparam name="T">The return type.</typeparam>
    /// <exception cref="ArgumentException"></exception>
    member self.GetValue<'T>() = 
        raise <| System.NotImplementedException()
        // does not work in most cases, maybe pattern matching with typeof<'T> and then conversion?
        //try box value |> unbox<'T>
        //with :? System.ArgumentException -> failwith $"Cannot convert FsCell of DataType {self.DataType} to type {typeof<'T>}"

    /// Returns the FsCells value.
    member self.GetValue() = _cellValue

    member self.InsertCellsAbove(numberOfRows) = raise (System.NotImplementedException())
    
    member self.InsertCellsAfter(numberOfColumns) = raise (System.NotImplementedException())
    
    member self.InsertCellsBefore(numberOfColumns) = raise (System.NotImplementedException())
    
    member self.InsertCellsBelow(numberOfRows)  = raise (System.NotImplementedException())
    
    /// <summary>
    /// Inserts the IEnumerable data elements and returns the range it occupies.
    /// </summary>
    /// <param name="data">The IEnumerable data.</param>
    member self.InsertData(data)  = raise (System.NotImplementedException())
    
    /// <summary>
    /// Inserts the IEnumerable data elements and returns the range it occupies.
    /// </summary>
    /// <param name="data">The IEnumerable data.</param>
    /// <param name="transpose">if set to <c>true</c> the data will be transposed before inserting.</param>
    /// <returns></returns>
    member self.InsertData(data, transpose) = raise (System.NotImplementedException())
    
    ///// <summary>
    ///// Inserts the data of a data table.
    ///// </summary>
    ///// <param name="dataTable">The data table.</param>
    ///// <returns>The range occupied by the inserted data</returns>
    //member self.InsertData(dataTable) = raise (System.NotImplementedException())
    
    /// <summary>
    /// Inserts the IEnumerable data elements as a table and returns it.
    /// <para>The new table will receive a generic name: Table#</para>
    /// </summary>
    /// <param name="data">The table data.</param>
    member self.InsertTable<'T>(data) = raise (System.NotImplementedException())
    
    ///// <summary>
    ///// Inserts the IEnumerable data elements as a table and returns it.
    ///// <para>The new table will receive a generic name: Table#</para>
    ///// </summary>
    ///// <param name="data">The table data.</param>
    ///// <param name="createTable">
    ///// if set to <c>true</c> it will create an Excel table.
    ///// <para>if set to <c>false</c> the table will be created in memory.</para>
    ///// </param>
    //member self.InsertTable<'T>(data, createTable)  = raise (System.NotImplementedException())
    
    /// <summary>
    /// Creates an Excel table from the given IEnumerable data elements.
    /// </summary>
    /// <param name="data">The table data.</param>
    /// <param name="tableName">Name of the table.</param>
    member self.InsertTable<'T>(data, tableName) = raise (System.NotImplementedException())
    
    /// <summary>
    /// Inserts the IEnumerable data elements as a table and returns it.
    /// </summary>
    /// <param name="data">The table data.</param>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="createTable">
    /// if set to <c>true</c> it will create an Excel table.
    /// <para>if set to <c>false</c> the table will be created in memory.</para>
    /// </param>
    member self.InsertTable<'T>(data, tableName, createTable) = raise (System.NotImplementedException())
    
    /// <summary>
    /// Inserts the DataTable data elements as a table and returns it.
    /// <para>The new table will receive a generic name: Table#</para>
    /// </summary>
    /// <param name="data">The table data.</param>
    member self.InsertTable(data) = raise (System.NotImplementedException())
    
    ///// <summary>
    ///// Inserts the DataTable data elements as a table and returns it.
    ///// <para>The new table will receive a generic name: Table#</para>
    ///// </summary>
    ///// <param name="data">The table data.</param>
    ///// <param name="createTable">
    ///// if set to <c>true</c> it will create an Excel table.
    ///// <para>if set to <c>false</c> the table will be created in memory.</para>
    ///// </param>
    //member self.InsertTable(data, createTable) = raise (System.NotImplementedException())
    
    /// <summary>
    /// Creates an Excel table from the given DataTable data elements.
    /// </summary>
    /// <param name="data">The table data.</param>
    /// <param name="tableName">Name of the table.</param>
    member self.InsertTable(data, tableName)  = raise (System.NotImplementedException())
    
    /// <summary>
    /// Inserts the DataTable data elements as a table and returns it.
    /// </summary>
    /// <param name="data">The table data.</param>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="createTable">
    /// if set to <c>true</c> it will create an Excel table.
    /// <para>if set to <c>false</c> the table will be created in memory.</para>
    /// </param>
    member self.InsertTable(data, tableName, createTable) = raise (System.NotImplementedException())
    
    /// <summary>
    /// Invalidate <see cref="CachedValue"/> so the formula will be re-evaluated next time <see cref="Value"/> is accessed.
    /// If cell does not contain formula nothing happens.
    /// </summary>
    member self.InvalidateFormula() = raise (System.NotImplementedException())
    
    member self.IsEmpty() = raise (System.NotImplementedException())
    
    [<System.Obsolete("Use the overload with XLCellsUsedOptions")>]
    member self.IsEmpty(includeFormats) = raise (System.NotImplementedException())
    
    //member self.IsEmpty(options) = raise (System.NotImplementedException())
    
    member self.IsMerged() = raise (System.NotImplementedException())
    
    member self.MergedRange() = raise (System.NotImplementedException())
    
    member self.Select() = raise (System.NotImplementedException())
    
    member self.SetActive(value(* = true*)) = raise (System.NotImplementedException())
    
    /// <summary>
    /// Sets the type of this FsCell's data.
    /// <para>Changing the data type will cause FsSpreadsheet to convert the current value to the new DataType.</para>
    /// <para>An exception will be thrown if the current value cannot be converted to the new DataType.</para>
    /// </summary>
    /// <param name="dataType">Type of the data.</param>
    /// <returns></returns>
    member self.SetDataType(dataType) = 
        self.DataType <- dataType
    
    [<System.Obsolete("Use GetDataValidation to access the existing rule, or CreateDataValidation() to create a new one.")>]
    member self.SetDataValidation() = raise (System.NotImplementedException())
    
    member self.SetFormulaA1(formula) = raise (System.NotImplementedException())
    
    member self.SetFormulaR1C1(formula) = raise (System.NotImplementedException())
    
    member self.SetHyperlink(hyperlink) = raise (System.NotImplementedException())
    
    /// <summary>
    /// Sets the FsCell's value.
    /// <para>FsSpreadsheet will try to translate it to the corresponding type, if it cannot, the value will be left as a string.</para>
    /// </summary>
    /// <value>
    /// The object containing the value to set.
    /// </value>
    member self.SetValue<'T>(value) = 
        let t,v = DataType.InferCellValue value
        _dataType <- t
        _cellValue <- v
        self

    member self.TableCellType() = raise (System.NotImplementedException())
    
    //how 2:
    //return (format.ToUpper()) switch
            //{
            //    "A" => this.Address.ToString(),
            //    "F" => HasFormula ? this.FormulaA1 : string.Empty,
            //    "NF" => Style.NumberFormat.Format,
            //    "FG" => Style.Font.FontColor.ToString(),
            //    "BG" => Style.Fill.BackgroundColor.ToString(),
            //    "V" => GetFormattedString(),
            //    _ => throw new FormatException($"Format {format} was not recognised."),
            //};
    /// <summary>
    /// Returns a string that represents the current state of the FsCell according to the format.
    /// </summary>
    /// <param name="format">A: address, F: formula, NF: number format, BG: background color, FG: foreground color, V: formatted value</param>
    /// <returns></returns>
    member self.ToString(format) = raise (System.NotImplementedException())
    
    // same problem like with .GetValue<'T>
    member self.TryGetValue<'T>(value) = raise (System.NotImplementedException())

    // --------------
    // STATIC METHODS
    // --------------

    /// <summary>
    /// Returns a string that represents the current state of the FsCell according to the format.
    /// </summary>
    /// <param name="format">A: address, F: formula, NF: number format, BG: background color, FG: foreground color, V: formatted value</param>
    /// <returns></returns>
    static member toStringFormat format = raise (System.NotImplementedException())

    /// Copies and replaces DataType and Value from a source FsCell into a target FsCell.
    static member copy (sourceCell : FsCell) (targetCell : FsCell) =
        targetCell.DataType <- sourceCell.DataType
        targetCell.Value <- sourceCell.Value
        targetCell

    /// <summary>
    /// Converts an FsCell's value converted to Double (64-bit float).
    /// <para>FsSpreadsheet will try to convert the current value to Double.</para>
    /// <para>An exception will be thrown if the current value cannot be converted to Double.</para>
    /// </summary>
    static member getDouble (cell : FsCell) =
        try float cell.Value
        with _ -> failwith $"FsCell with DataType {cell.DataType} cannot be converted to Double."

    /// <summary>
    /// Sets the type of an FsCell's data.
    /// <para>Changing the data type will cause FsSpreadsheet to convert the current value to the new DataType.</para>
    /// <para>An exception will be thrown if the current value cannot be converted to the new DataType.</para>
    /// </summary>
    /// <param name="dataType">Type of the data.</param>
    /// <returns></returns>
    static member setDataType dataType (cell : FsCell) =
        cell.DataType <- dataType
        cell

    /// <summary>
    /// Sets an FsCell's value.
    /// <para>FsSpreadsheet will try to translate it to the corresponding type, if it cannot, the value will be left as a string.</para>
    /// </summary>
    /// <value>
    /// The object containing the value to set.
    /// </value>
    static member setValue<'T> value (cell : FsCell)= 
        let t,v = DataType.InferCellValue value
        cell.DataType <- t
        cell.Value <- v
        cell