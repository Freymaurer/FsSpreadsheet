﻿namespace FsSpreadsheet

open System.Collections.Generic

type FsTable (name : string, rangeAddress, showTotalsRow, showHeaderRow) = 

    inherit FsRangeBase(rangeAddress)

    let mutable _name = name

    let mutable _lastRangeAddress = rangeAddress
    let mutable _showTotalsRow : bool = showTotalsRow
    let mutable _showHeaderRow : bool = showHeaderRow

    let mutable _fieldNames : Dictionary<string,FsTableField> = Dictionary()
    let _uniqueNames : HashSet<string> = HashSet()

    new (name, rangeAddress, showHeaderRow) = FsTable (name, rangeAddress, false, showHeaderRow)

    new (name, rangeAddress) = FsTable (name, rangeAddress, false, true)

    /// <summary>The name of the FsTable.>/summary>
    member self.Name 
        with get() = _name

    /// <summary>Returns all fieldnames as `fieldname*FsTableField` dictionary.</summary>
    member self.FieldNames
        with get(cellsCollection) =
            if (_fieldNames <> null && _lastRangeAddress <> null && _lastRangeAddress.Equals(self.RangeAddress)) then 
                _fieldNames;
            else 
                _lastRangeAddress <- self.RangeAddress

                //self.RescanFieldNames(cellsCollection)
                
                _fieldNames;

    /// <summary>The FsTableFields of this FsTable.<summary>
    member self.Fields
        with get(cellsCollection) =
            let columnCount = base.ColumnCount()
            let offset = base.RangeAddress.FirstAddress.ColumnNumber
            Seq.init columnCount (fun i -> self.GetField(i + offset, cellsCollection))

    /// <summary>Gets or sets if the header row is shown.</summary>
    member self.ShowHeaderRow 
        with get () = _showHeaderRow
        and set(showHeaderRow) = _showHeaderRow <- showHeaderRow

    /// <summary>Returns the header row as FsRangeRow. Scans for fieldnames if `scanForNewFieldsNames` is true.</summary>
    member self.HeadersRow(scanForNewFieldsNames : bool) = 
        if (not self.ShowHeaderRow) then null;
        
        else 
            //if (scanForNewFieldsNames) then
        
            //    let tempResult = this.FieldNames;
            //    ()

            FsRange(base.RangeAddress).FirstRow();

    /// <summary>Returns the header row as FsRangeRow. Scans for new fieldnames.</summary>
    member self.HeadersRow() = 
        self.HeadersRow(true)

    /// Takes the respective FsCellsCollection for this FsTable and creates a new _fieldNames dictionary if the current one does not match.</summary>
    // TO DO: maybe HLW can specify above description a bit...
    member private self.RescanFieldNames(cellsCollection : FsCellsCollection) =
        printfn "Start RescanFieldNames"
        _fieldNames
        |> Seq.iter (fun kv -> printfn "Key: %s, index: %i, name: %s" kv.Key kv.Value.Index kv.Value.Name)
        if self.ShowHeaderRow then
            let oldFieldNames =  _fieldNames
            _fieldNames <- new Dictionary<string, FsTableField>()
            let headersRow = self.HeadersRow(false);
            let mutable cellPos = 0
            for cell in headersRow.Cells(cellsCollection) do
                let mutable name = cell.Value //GetString();
                match Dictionary.tryGet name oldFieldNames with
                | Some tableField ->
                    tableField.Index <- cellPos
                    _fieldNames.Add(name,tableField)
                    cellPos <- cellPos + 1
                | None -> 

                    // Be careful here. Fields names may actually be whitespace, but not empty
                    if (name = null) <> (name = "") then    // TO DO: ask: shouldn't this be XOR?
                    
                        name <- self.GetUniqueName("Column", cellPos + 1, true)
                        cell.SetValueAs(name) |> ignore
                        cell.DataType <- DataType.String

                    if (_fieldNames.ContainsKey(name)) then
                        raise (System.ArgumentException("The header row contains more than one field name '" + name + "'."))

                    _fieldNames.Add(name, new FsTableField(name, cellPos))
                    cellPos <- cellPos + 1
        else
            
            let colCount = base.ColumnCount();
            for i = 1 to colCount (**i <= colCount**) do

                if _fieldNames.Values |> Seq.exists (fun v -> v.Index = i - 1) |> not then //.All(f => f.Index != i - 1)) then

                    let name = "Column" + string i;

                    _fieldNames.Add(name, new FsTableField(name, i - 1));

        printfn "Finished RescanFWieldNames"
        _fieldNames
        |> Seq.iter (fun kv -> printfn "Key: %s, index: %i, name: %s" kv.Key kv.Value.Index kv.Value.Name)

    /// <summary>
    /// Updates the FsRangeAddress of the FsTable according to the FsTableFields associated.
    /// </summary>
    member self.RescanRange() =
        let rangeAddress = 
            _fieldNames.Values
            |> Seq.map (fun v -> v.Column.RangeAddress)
            |> Seq.reduce (fun r1 r2 -> r1.Union(r2))
        base.RangeAddress <- rangeAddress

    /// <summary>
    /// Updates the FsRangeAddress of a given FsTable according to the FsTableFields associated.
    /// </summary>
    static member rescanRange (table : FsTable) =
        table.RescanRange()

    /// <summary>
    /// Returns a unique name consisting of the original name and an initial offset that is raised 
    /// if the original name with that offset is already present.
    /// </summary>
    /// <param name="enforceOffset">
    /// If true, the initial offset is always 
    /// </param>
    // TO DO: HLW: make this description more precise. What is this method even about?
    member this.GetUniqueName(originalName : string, initialOffset : int32, enforceOffset : bool) =
        let mutable name = originalName + if enforceOffset then string initialOffset else ""
        if _uniqueNames.Contains(name) then
        
            let mutable i = initialOffset
            name <- originalName + string i
            while _uniqueNames.Contains(name) do

                i <- i + 1
                name <- originalName + string i

        _uniqueNames.Add name |> ignore
        name

    static member getUniqueNames originalName initialOffset enforceOffset (table : FsTable) =
        table.GetUniqueName(originalName, initialOffset, enforceOffset)

    member this.AddFields(fieldNames : seq<string>) =
    
    //    _fieldNames = new Dictionary<String, IXLTableField>();

    //    Int32 cellPos = 0;
        let mutable cellPos = 0
        let range = base.RangeAddress.FirstAddress. // NEED: FsRangeBase.toFsRangeColumns
        fieldNames
        |> Seq.iter (
            fun fn ->
                _fieldNames.Add(fn, FsTableField())
        )
    //    foreach (var name in fieldNames)
    //    {
    //        _fieldNames.Add(name, new XLTableField(this, name) { Index = cellPos++ });
    //    }

    /// <summary>Returns the FsTableField with given name. If an FsTableField does not exist under this name in the FsTable, adds it.</summary>
    member self.Field(name : string, cellsCollection : FsCellsCollection) = 
        match Dictionary.tryGet name _fieldNames with
        | Some field -> 
            field
        | None -> 
            let maxIndex = 
                _fieldNames.Values 
                |> Seq.map (fun v -> v.Index) 
                |> fun s -> 
                    if Seq.length s = 0 then 0 else Seq.max s
            let range = 
                let offset = _fieldNames.Count
                let firstAddress = FsAddress(self.RangeAddress.FirstAddress.RowNumber,self.RangeAddress.FirstAddress.ColumnNumber + offset)
                let lastAddress = FsAddress(self.RangeAddress.LastAddress.RowNumber,self.RangeAddress.FirstAddress.ColumnNumber + offset)
                FsRangeAddress(firstAddress,lastAddress)
            let column = FsRangeColumn(range)
            let newField = FsTableField(name,maxIndex + 1,column,null,null)
            if self.ShowHeaderRow then
                newField.HeaderCell(cellsCollection,true).SetValueAs name |> ignore
            _fieldNames.Add(name,newField)
            self.RescanRange()
            newField

    /// <summary>Takes a name of an FsTableField and an FsCellsCollection (belonging to the FsWorksheet of this FsTable) and returns the respective FsTableField.</summary>
    /// <exception cref="System.ArgumentException">if the header row has no field with the given name.</exception>
    member self.GetField(name : string, cellsCollection : FsCellsCollection) =
        let name = name.Replace("\r\n", "\n")
        try self.FieldNames(cellsCollection).Item name
        with _ -> failwith <| "The header row doesn't contain field name '" + name + "'."

    /// <summary>Takes a name of an FsTableField and an FsCellsCollection (belonging to the FsWorksheet of this FsTable) and returns the respective FsTableField.</summary>
    /// <exception cref="System.ArgumentException">if the header row has no field with the given name.</exception>
    static member getFieldByName (name : string) (cellsCollection : FsCellsCollection) (table : FsTable) =
        table.GetField(name, cellsCollection)

    /// <summary>Takes the index of an FsTableField and an FsCellsCollection (belonging to the FsWorksheet of this FsTable) and returns the respective FsTableField.</summary>
    /// <exception cref="System.ArgumentException">if the FsTable has no FsTableField with the given index.</exception>
    member self.GetField(index, cellsCollection) =
        try 
            self.FieldNames(cellsCollection).Values
            |> Seq.find (fun ftf -> ftf.Index = index)
        with _ -> failwith $"FsTableField with index {index} does not exist in the FsTable."

    /// <summary>Takes a name of an FsTableField and an FsCellsCollection (belonging to the FsWorksheet of this FsTable) and returns the index of the respective FsTableField.</summary>
    /// <exception cref="System.ArgumentException">if the header row has no field with the given name.</exception>
    member self.GetFieldIndex(name : string, cellsCollection) =
        self.GetField(name, cellsCollection).Index

    /// <summary>Renames a fieldname of the FsTable if it exists. Else fails.</summary>
    /// <exception cref="System.ArgumentException">if the FsTableField does not exist in the FsTable.</exception>
    member this.RenameField(oldName : string, newName : string) = 
        match Dictionary.tryGet oldName _fieldNames with
        | Some field -> 
            _fieldNames.Remove(oldName) |> ignore
            _fieldNames.Add(newName, field)
        | None -> 
            raise (System.ArgumentException("The FsTabelField does not exist in this FsTable", "oldName"))

    /// <summary>Renames a fieldname of the FsTable if it exists. Else fails.</summary>
    /// <exception cref="System.ArgumentException">if the FsTableField does not exist in the FsTable.</exception>
    static member renameField oldName newName (table : FsTable) =
        table.RenameField(oldName, newName)