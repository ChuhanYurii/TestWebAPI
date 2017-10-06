function ViewModel() {
    var self = this;

    var tokenKey = 'accessToken';

    self.result = ko.observable();
    self.user = ko.observable();

    self.registerEmail = ko.observable();
    self.registerPassword = ko.observable();
    self.registerPassword2 = ko.observable();

    self.loginEmail = ko.observable();
    self.loginPassword = ko.observable();
    self.errors = ko.observableArray([]);

    function showError(jqXHR) {

        self.result(jqXHR.status + ': ' + jqXHR.statusText);

        var response = jqXHR.responseJSON;
        if (response) {
            if (response.Message) self.errors.push(response.Message);
            if (response.ModelState) {
                var modelState = response.ModelState;
                for (var prop in modelState) {
                    if (modelState.hasOwnProperty(prop)) {
                        var msgArr = modelState[prop]; // expect array here
                        if (msgArr.length) {
                            for (var i = 0; i < msgArr.length; ++i) self.errors.push(msgArr[i]);
                        }
                    }
                }
            }
            if (response.error) self.errors.push(response.error);
            if (response.error_description) self.errors.push(response.error_description);
        }
    }

    self.callApi = function () {
        self.result('');
        self.errors.removeAll();

        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'GET',
            url: '/api/values',
            headers: headers
        }).done(function (data) {
            self.result(data);
        }).fail(showError);
    }

    self.register = function () {
        self.result('');
        self.errors.removeAll();

        var data = {
            Email: self.registerEmail(),
            Password: self.registerPassword(),
            ConfirmPassword: self.registerPassword2()
        };

        $.ajax({
            type: 'POST',
            url: '/api/Account/Register',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data)
        }).done(function (data) {
            self.result("Done!");
        }).fail(showError);
    }

    self.login = function () {
        self.result('');
        self.errors.removeAll();

        var loginData = {
            grant_type: 'password',
            username: self.loginEmail(),
            password: self.loginPassword()
        };

        $.ajax({
            type: 'POST',
            url: '/Token',
            data: loginData
        }).done(function (data) {
            self.user(data.userName);
            // Cache the access token in session storage.
            sessionStorage.setItem(tokenKey, data.access_token);
        }).fail(showError);
    }

    self.logout = function () {
        // Log out from the cookie based logon.
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'POST',
            url: '/api/Account/Logout',
            headers: headers
        }).done(function (data) {
            // Successfully logged out. Delete the token.
            self.user('');
            sessionStorage.removeItem(tokenKey);
        }).fail(showError);
    }


    self.folders = ko.observableArray();
    self.error = ko.observable();
    self.detail = ko.observable();
    self.users = ko.observableArray();
    self.newFolder = {
        User: ko.observable(),
        UserName: ko.observable(),
        Name: ko.observable(),
        Id: ko.observable(),
        UserId: ko.observable()
    }

    var foldersUri = '/api/Folder/';
    var usersUri = '/api/Account/GetAllUsers';

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

    function getAllFolders() {
        ajaxHelper(foldersUri, 'GET').done(function (data) {
            self.folders(data);
        });
    }

    self.getFolderDetail = function (item) {
        ajaxHelper(foldersUri + item.Id, 'GET').done(function (data) {
            self.detail(data);
        });
    }

    function getUsers() {
        ajaxHelper(usersUri, 'GET').done(function (data) {
            self.users(data);
        });
    }


    self.addFolder = function (formElement) {
        var folder = {
            UserId: self.newFolder.User().ID,
            Name: self.newFolder.Name()
        };

        ajaxHelper(foldersUri, 'POST', folder).done(function (item) {
            self.folders.push(item);
        });
    }

    self.editFolder = function (formElement) {
        var folder = {
            UserId: self.detail().UserId,
            Name: self.detail().Name,
            Id: self.detail().Id
        };

        ajaxHelper(foldersUri + folder.Id, 'PUT', folder).done(function (item) {
            getAllFolders();
        });
    }

    self.deleteFolder = function (item) {
        ajaxHelper(foldersUri + item.Id, 'DELETE').done(function (data) {
            getAllFolders();
        });
    }

    // Fetch the initial data.
    getAllFolders();
    getUsers();
}

var app = new ViewModel();
ko.applyBindings(app);