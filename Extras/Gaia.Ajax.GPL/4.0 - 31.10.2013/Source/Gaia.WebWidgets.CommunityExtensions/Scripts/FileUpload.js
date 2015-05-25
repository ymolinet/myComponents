// Gaia Ajax Copyright (C) 2008 - 2011 Gaiaware AS. details at http://gaiaware.net/

/* 
 * Gaia Ajax - Ajax Control Library for ASP.NET
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved.
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by
 * Gaiaware AS
 * read the details at http://gaiaware.net
 */

/* ---------------------------------------------------------------------------
   Ajax FileUpload Control 
   Author : Pavol Rusanov, Czech 
   --------------------------------------------------------------------------- */

if (!Gaia.Extensions)
    Gaia.Extensions = Class.create();

Gaia.Extensions.FileUpload = jsface.Class(Gaia.Panel, {
    constructor: function(element, options) {
        Gaia.Extensions.FileUpload.$super.call(this, element, options);
        this.initializeFileUpload(element, options);
    },

    initializeFileUpload: function(element, options) {

        // disable upload button a catch click event
        var btn = $(this.options.btnID);
        btn.FUP = this;
        btn.disabled = true;
        Event.observe(btn, "click", this.uploadClick);


        this.element[0].setAttribute('name', 'fup');
        this.element[0].FUP = this;
        this.options.actualFU = -1;
        this.options.fileRecords = new Array();
        this.options.filesUploaded = false;

        this.createFiles();

        var emptyFile = this.getEmptyObjFile();
        if (emptyFile != null) {
            this.options.actualFile = this.createInputFile(emptyFile.id, emptyFile.name);
            this.options.actualFile.disabled = !this.options.enabled;
        }

        $(this.options.btnCallbackID).disabled = false;

    },

    // create array of file ids
    createFiles: function() {
        this.options.files = new Array();

        for (var i = 0; i < this.options.maxFiles; i++) {
            var file = $(this.element[0].id + "_f" + i);
            var objFile = new Object();
            objFile.id = file.id;
            objFile.name = file.name;
            objFile.empty = true;
            this.options.files[i] = objFile;
            file.parentNode.removeChild(file);
        }
    },
    // create an empty INPUT element (file)
    createInputFile: function(id, name) {
        var file = document.createElement('input');
        file.type = 'file';
        file.id = id;
        file.name = name;
        this.element[0].insertBefore(file, $(this.options.btnID));
        file.FUP = this;
        this.options.actualFile = file;
        Event.observe(file, "change", this.fileChanged);
        return file;
    },
    // the file has been chosen, create some info about in DIV below
    createFileRecord: function(file) {

        var css = this.element[0].className;

        var container = $(this.options.divFilesID);

        var div = document.createElement('div');
        div.file = file;

        var textSpan = document.createElement('span');
        textSpan.className = css + '-fileupload-file-text';
        textSpan.innerHTML = file.value;
        div.appendChild(textSpan);

        var btnDel = document.createElement('input');
        btnDel.type = 'submit';
        btnDel.value = this.options.tDelete;
        btnDel.className = css + '-fileupload-button-delete';
        btnDel.div = div;
        btnDel.FUP = this;
        Event.observe(btnDel, "click", this.btnDelClick);
        div.appendChild(btnDel);
        div.btnDel = btnDel;

        if (this.options.imgSrc != "") {
            var imgLoading = document.createElement('img');
            imgLoading.style.display = 'none';
            imgLoading.setAttribute("alt", "loading...");
            imgLoading.setAttribute("src", this.options.imgSrc);
            imgLoading.className = css + '-fileupload-img-loading';
            div.appendChild(imgLoading);
            div.imgSrc = imgLoading;
        }

        var infoSpan = document.createElement('span');
        infoSpan.className = css + '-fileupload-info-text';
        div.appendChild(infoSpan);
        div.infoSpan = infoSpan;

        container.appendChild(div);
        return div;
    },

    // the file has been chosen
    fileChanged: function() {

        $(this.FUP.options.btnID).disabled = false;
        this.style.display = 'none';

        if (this.FUP.options.filesUploaded) {
            this.FUP.deleteFileRecords();
            this.FUP.options.filesUploaded = false;
        }

        var div = this.FUP.createFileRecord(this);

        this.FUP.setObjFileEmpty(this.id, false);
        this.FUP.options.fileRecords[this.FUP.options.fileRecords.length] = div;

        var emptyFile = this.FUP.getEmptyObjFile();
        if (emptyFile != null) {
            this.FUP.options.actualFile = this.FUP.createInputFile(emptyFile.id, emptyFile.name);
        } else {
            this.FUP.options.actualFile = this.FUP.createInputFile(this.FUP.element.id + "ff", this.FUP.element.id + "ff");
            this.FUP.options.actualFile.disabled = true;
        }
    },
    // delete button has been clicked
    btnDelClick: function() {
        var container = $(this.FUP.options.divFilesID);
        container.removeChild(this.div);

        this.FUP.options.fileRecords.remove(this.div);
        this.FUP.setObjFileEmpty(this.div.file.id, true);
        this.div.file.parentNode.removeChild(this.FUP.options.actualFile);
        this.div.file.parentNode.removeChild(this.div.file);

        var emptyFile = this.FUP.getEmptyObjFile();
        if (emptyFile != null) {
            this.FUP.createInputFile(emptyFile.id, emptyFile.name);
        }
    },
    // upload button has been clicked or another file has to be uploaded
    uploadClick: function(infoText) {

        var fup = this.FUP == null ? this : this.FUP;

        var isMaxReached = fup.hideFileUploads(infoText);
        // is any file waiting in the list that has to be uploaded?
        if (!isMaxReached) {

            fup.prepareAndSubmitForm();

            var executer = new PeriodicalExecuter(fup.isUploadCompleted, 1);
            executer.FUP = fup;
        } else {
            fup.options.actualFU = -1;
            fup.deleteFileInputs(true);
            fup.setAllObjFileEmpty(true);
            fup.recoverOtherElementsFileUploads();
            fup.options.filesUploaded = true;
            fup.options.fileRecords.length = 0;
            var emptyFile = fup.getEmptyObjFile();
            if (emptyFile != null)
                fup.createInputFile(emptyFile.id, emptyFile.name);

            if (fup.options.uploadOnce)
                fup.options.actualFile.disabled = true;

            if (fup.options.makeCallback)
                $(fup.options.btnCallbackID).click();
        }
    },

    // some attributes that has to be filled
    prepareAndSubmitForm: function() {
        document.forms[0].target = this.options.frameID;
        document.forms[0].enctype = 'multipart/form-data';
        document.forms[0].encoding = 'multipart/form-data'; // IE hack
        //$(this.options.lblInfoID).innerHTML = this.options.tUploading;
        $(this.options.hfID).value = '1';

        this.options.lastTitle = 'blank';

        $(this.options.btnID).disabled = true;
        document.forms[0].submit();
    },
    // timer has ticked
    isUploadCompleted: function() {
        try {
            var frame = $(this.FUP.options.frameID);
            var newTitle = frame.contentWindow.document.title;
            // no message from server yet
            if (this.FUP.options.lastTitle == newTitle) {
                // contine timing...
            }
                // the file has been successfully uploaded
            else if (newTitle.indexOf('upload') != -1) {
                this.stop();
                document.forms[0].target = '';
                $(this.FUP.options.hfID).value = '0';
                frame.contentWindow.document.title = 'blank';

                var titles = newTitle.split('|');
                if (titles.length == 2)
                    this.FUP.uploadClick(titles[1]);
            } else throw ('error');
        }
            // the size of the file has been probably exceeded
        catch(err) {
            this.stop();
            frame.src = this.FUP.options.blankPath;
            document.forms[0].target = '';
            $(this.FUP.options.hfID).value = '0';
            this.FUP.uploadClick(this.FUP.options.tSizeError);
        }
    },
    // server wants to Enable/Disable the control
    onEnabledChanged: function(enabled) {
        this.options.actualFile.disabled = !enabled;
        return this;
    },
    // from here it's the helper functions like hiding, recovering file controls...
    setObjFileEmpty: function(id, empty) {
        for (var i = 0; i < this.options.files.length; i++) {
            if (this.options.files[i].id == id) {
                this.options.files[i].empty = empty;
                break;
            }
        }
    },

    setAllObjFileEmpty: function(empty) {
        for (var i = 0; i < this.options.files.length; i++) {
            this.options.files[i].empty = empty;
        }
    },

    getEmptyObjFile: function() {
        for (var i = 0; i < this.options.files.length; i++) {
            if (this.options.files[i].empty)
                return this.options.files[i];
        }
        return null;
    },

    deleteFileInputs: function(deleteActual) {
        var btn = $(this.options.btnID);
        for (var i = 0; i < this.options.fileRecords.length; i++) {
            var file = this.options.fileRecords[i].file;
            if (file.parentNode != null)
                file.parentNode.removeChild(file);
        }
        if (deleteActual && this.options.actualFile.parentNode != null)
            this.options.actualFile.parentNode.removeChild(this.options.actualFile);
    },

    recoverFileInputs: function() {
        var btn = $(this.options.btnID);
        for (var i = 0; i < this.options.fileRecords.length; i++) {
            var file = this.options.fileRecords[i].file;
            if (file.parentNode == null)
                this.element[0].insertBefore(file, btn);
        }
    },

    deleteFileRecords: function() {
        var divFiles = document.getElementById(this.options.divFilesID);
        if (divFiles.hasChildNodes()) {
            while (divFiles.childNodes.length >= 1 && divFiles.lastChild.tagName.toLowerCase() == 'div') {
                divFiles.removeChild(divFiles.lastChild);
            }
        }
        this.options.fileRecords.length = 0;
    },

    hideFileUploads: function(infoText) {

        if (this.options.actualFU > -1) {
            this.options.fileRecords[this.options.actualFU].infoSpan.innerHTML = infoText;
            if (this.options.imgSrc != "")
                this.options.fileRecords[this.options.actualFU].imgSrc.style.display = 'none';
        } else {
            this.hideOtherElementsFileUploads();
        }

        this.options.actualFU++;
        this.deleteFileInputs(false);

        if (this.options.fileRecords.length > this.options.actualFU) {
            var div = this.options.fileRecords[this.options.actualFU];

            this.element[0].insertBefore(div.file, $(this.options.btnID));
            div.infoSpan.innerHTML = this.options.tUploading;
            div.btnDel.disabled = true;
            if (this.options.imgSrc != "")
                div.imgSrc.style.display = '';
            return false;
        }
        return true;
    },

    hideOtherElementsFileUploads: function() {

        var FUPs = this.getElementsByName_iefix('div', 'fup');
        for (var i = 0; i < FUPs.length; i++) {
            if (FUPs[i].id != this.element[0].id)
                FUPs[i].FUP.deleteFileInputs(false);
        }
    },

    recoverOtherElementsFileUploads: function() {

        var FUPs = this.getElementsByName_iefix('div', 'fup');
        for (var i = 0; i < FUPs.length; i++) {
            if (FUPs[i].id != this.element[0].id)
                FUPs[i].FUP.recoverFileInputs();
        }
    },

    getElementsByName_iefix: function(tag, name) {

        var elem = document.getElementsByTagName(tag);
        var arr = new Array();
        for (i = 0, iarr = 0; i < elem.length; i++) {
            att = elem[i].getAttribute("name");
            if (att == name) {
                arr[iarr] = elem[i];
                iarr++;
            }
        }
        return arr;
    }
});

Array.prototype.remove = function(s) {
    for (i = 0; i < this.length; i++) {
        if (s == this[i]) this.splice(i, 1);
    }
}

