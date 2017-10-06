function ViewModel() {
    var self = this;

    self.files = ko.observableArray();
    self.error = ko.observable();
    self.detail = ko.observable();
    self.folders = ko.observableArray();
    self.newFile = {
        User: ko.observable(),
        UserName: ko.observable(),
        Name: ko.observable(),
        Id: ko.observable(),
        UserId: ko.observable(),
        Size: ko.observable(),
        FileExtension: ko.observable(),
        Path: ko.observable(),
        Folder: ko.observable(),
        FolderName: ko.observable(),
        FolderId: ko.observable()
    }

    var filesUri = '/api/File/';
    var foldersUri = '/api/Folder/';

    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
            
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    function getAllFiles() {
        ajaxHelper(filesUri, 'GET').done(function (data) {
            self.files(data);
        });
    }

    self.getFileDetail = function (item) {
        ajaxHelper(filesUri + item.Id, 'GET').done(function (data) {
            self.detail(data);
        });
    }

    function getFolders() {
        ajaxHelper(foldersUri, 'GET').done(function (data) {
            self.folders(data);
        });
    }


    self.addFile = function (formElement) {
        var file = {
            FolderId: self.newFile.Folder().Id            
        };

        ajaxHelper(filesUri, 'POST', file).done(function (item) {
            self.files.push(item);
        });
    }

    self.editFile = function (formElement) {
        var file = {
            FolderId: self.detail().FolderId,
            Name: self.detail().Name,
            Id: self.detail().Id,
            Size: self.detail().Size,
            FileExtension: self.detail().FileExtension,
            Path: self.detail().Path
        };

        ajaxHelper(filesUri + file.Id, 'PUT', file).done(function (item) {
            getAllFiles();
        });
    }

    self.deleteFile = function (item) {
        ajaxHelper(filesUri + item.Id, 'DELETE').done(function (data) {
            getAllFiles();
        });
    }

    // Fetch the initial data.
    getAllFiles();
    getFolders();
}

var appFile = new ViewModel();
ko.applyBindings(appFile);