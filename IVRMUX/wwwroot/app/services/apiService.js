
app.service('apiService', ['$http', '$q', 'appSettings', '$window', '$state', '$cookieStore', function ($http, $q, appSettings, $window, $state, $cookieStore) {
    var apiService = {};
    var apiBase = appSettings.apiBase;
    //===========================GET RESOURCE==============================
    var get = function (module, parameter) {
        var deferred = $q.defer();
        if (module == "Login") {
            var authdata = "username=" + parameter.Username +
                "&password=" + parameter.password +
                "&ClientId=" + parameter.ClientId +
                "&Logintype=" + parameter.Logintype +
                "&grant_type=password";
            // gettoken("Authorization/connect/token", authdata);
            $http.post(apiBase + "Authorization/connect/token", authdata,
                { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                .success(function (promise) {
                    if (promise.error === undefined) {
                        localStorage.setItem('accesstoken', promise.access_token);
                        localStorage.setItem('refresh_token', promise.refresh_token);
                        var timestamp = Math.round(new Date() / 1000) + promise.expires_in;
                        localStorage.setItem('session_expiresin', timestamp);
                        var session_expiresin = localStorage.getItem('session_expiresin');
                    }
                    //else {
                    //    swal(promise.error_description);
                    //    return;
                    //}
                    deferred.resolve(promise);
                })
                .error(function (promise) {
                    deferred.resolve(promise);
                });
            //
        }
        else {
            $http.get(apiBase + module, { params: parameter }, { headers: { 'Content-Type': 'application/json' } }).success(function (response) {
                deferred.resolve(response);

            }).catch(function (data, status, headers, config) { // <--- catch instead error
                deferred.reject(data.statusText);
            });
        }
        return deferred.promise;
    };

    //===========================GET DATA==============================
    var getDATA = function (module) {
        var deferred = $q.defer();
        $http.get(apiBase + module, { headers: { 'Content-Type': 'application/json' } }).success(function (response) {
            if (response.isSessionExpired == true) {
                $cookieStore.remove('pagetitle');
                $cookieStore.put("pagetitle", "Home");
                $cookieStore.remove('IsLogged');
                apiService.getURI("Login/clearsession", 1).
                    then(function (promise) {
                        //$state.go("login");
                        if (promise.subDomainNamelogout != null && promise.subDomainNamelogout != "") {
                            window.location.href = "http://localhost:57606/#/login/" + promise.subDomainNamelogout;
                        }
                        else {
                            window.location.href = "http://localhost:57606/#/login/";
                        }
                    })
                swal("Your Session has been Expired.....Please Re-login");
            }
            deferred.resolve(response);
        }).catch(function (data, status, headers, config) { // <--- catch instead error
            deferred.reject(data.statusText);
        });
        return deferred.promise;
    };

    //===========================GET RESOURCE==============================
    var getURI = function (module, parameter) {
        var deferred = $q.defer();
        $http.get(apiBase + module + "/" + parameter, { headers: { 'Content-Type': 'application/json' } }).success(function (response) {
            if (response.isSessionExpired == true) {
                $cookieStore.remove('pagetitle');
                $cookieStore.put("pagetitle", "Home");
                $cookieStore.remove('IsLogged');
                apiService.getURI("Login/clearsession", 1).
                    then(function (promise) {
                        //$state.go("login");
                        if (promise.subDomainNamelogout != null && promise.subDomainNamelogout != "") {
                            window.location.href = "http://localhost:57606/#/login/" + promise.subDomainNamelogout;
                        }
                        else {
                            window.location.href = "http://localhost:57606/#/login/";
                        }
                    })
                swal("Your Session has been Expired.....Please Re-login");
            }
            deferred.resolve(response);
        }).catch(function (data, status, headers, config) { // <--- catch instead error
            deferred.reject(data.statusText);
        });
        return deferred.promise;
    };
    var create = function (module, parameter) {
        console.log("hitting Service=============");
        var deferred = $q.defer();
        $http.post(apiBase + module, parameter, { headers: { 'Content-Type': 'application/json' } }).success(function (response) {
            if (response.isSessionExpired == true) {
                $cookieStore.remove('pagetitle');
                $cookieStore.put("pagetitle", "Home");
                $cookieStore.remove('IsLogged');
                apiService.getURI("Login/clearsession", 1).
                    then(function (promise) {
                        //$state.go("login");
                        if (promise.subDomainNamelogout != null && promise.subDomainNamelogout != "") {
                            window.location.href = "http://localhost:57606/#/login/" + promise.subDomainNamelogout;
                        }
                        else {
                            window.location.href = "http://localhost:57606/#/login/";
                        }
                    })
                swal("Your Session has been Expired.....Please Re-login");
            }
            deferred.resolve(response);
        }).catch(function (data, status, headers, config) { // <--- catch instead error
            deferred.reject(data.statusText);
        });
        return deferred.promise;
    };

    //===========================UPDATE RESOURCE==============================
    var update = function (module, parameter) {
        console.log("hitting Service=============");
        var deferred = $q.defer();
        $http.post(apiBase + module + '/' + parameter.id, parameter, { headers: { 'Content-Type': 'application/json' } }).success(function (response) {
            if (response.isSessionExpired == true) {
                $cookieStore.remove('pagetitle');
                $cookieStore.put("pagetitle", "Home");
                $cookieStore.remove('IsLogged');
                apiService.getURI("Login/clearsession", 1).
                    then(function (promise) {
                        //$state.go("login");
                        if (promise.subDomainNamelogout != null && promise.subDomainNamelogout != "") {
                            window.location.href = "http://localhost:57606/#/login/" + promise.subDomainNamelogout;
                        }
                        else {
                            window.location.href = "http://localhost:57606/#/login/";
                        }
                    })
                swal("Your Session has been Expired.....Please Re-login");
                //$state.go("login");               
            }
            deferred.resolve(response);
        }).catch(function (data, status, headers, config) { // <--- catch instead error
            deferred.reject(data.statusText);
        });
        return deferred.promise;
    };
    //===========================DELETE RESOURCE==============================
    var delet = function (module, parameter) {
        console.log("hitting Service=============");
        var deferred = $q.defer();
        $http.delete(apiBase + module + '/' + parameter.id, parameter, { headers: { 'Content-Type': 'application/json' } }).success(function (response) {
            if (response.isSessionExpired == true) {
                $cookieStore.remove('pagetitle');
                $cookieStore.put("pagetitle", "Home");
                $cookieStore.remove('IsLogged');
                apiService.getURI("Login/clearsession", 1).
                    then(function (promise) {
                        //$state.go("login");
                        if (promise.subDomainNamelogout != null && promise.subDomainNamelogout != "") {
                            window.location.href = "http://localhost:57606/#/login/" + promise.subDomainNamelogout;
                        }
                        else {
                            window.location.href = "http://localhost:57606/#/login/";
                        }
                    })
                swal("Your Session has been Expired.....Please Re-login");
            }
            deferred.resolve(response);
        }).catch(function (data, status, headers, config) { // <--- catch instead error
            deferred.reject(data.statusText);
        });
        return deferred.promise;
    };

    //===========================GET RESOURCE==============================
    var DeleteURI = function (module, parameter) {
        var deferred = $q.defer();
        $http.delete(apiBase + module + "/" + parameter, { headers: { 'Content-Type': 'application/json' } }).success(function (response) {
            if (response.isSessionExpired == true) {
                $cookieStore.remove('pagetitle');
                $cookieStore.put("pagetitle", "Home");
                $cookieStore.remove('IsLogged');
                apiService.getURI("Login/clearsession", 1).
                    then(function (promise) {
                        //$state.go("login");
                        if (promise.subDomainNamelogout != null && promise.subDomainNamelogout != "") {
                            window.location.href = "http://localhost:57606/#/login/" + promise.subDomainNamelogout;
                        }
                        else {
                            window.location.href = "http://localhost:57606/#/login/";
                        }
                    })
                swal("Your Session has been Expired.....Please Re-login");
            }
            deferred.resolve(response);
        }).catch(function (data, status, headers, config) { // <--- catch instead error
            deferred.reject(data.statusText);
        });
        return deferred.promise;
    };
    ////===========================gettoken Auth RESOURCE==============================
    function gettoken(module, parameter) {
        console.log("hitting Service=============");
        var deferred = $q.defer();
        $http.post(apiBase + module, parameter, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
            .success(function (promise) {
                if (response.isSessionExpired == true) {
                    $cookieStore.remove('pagetitle');
                    $cookieStore.put("pagetitle", "Home");
                    $cookieStore.remove('IsLogged');
                    apiService.getURI("Login/clearsession", 1).
                        then(function (promise) {
                            //$state.go("login");
                            if (promise.subDomainNamelogout != null && promise.subDomainNamelogout != "") {
                                window.location.href = "http://localhost:57606/#/login/" + promise.subDomainNamelogout;
                            }
                            else {
                                window.location.href = "http://localhost:57606/#/login/";
                            }
                        })
                    swal("Your Session has been Expired.....Please Re-login");
                }
                if (promise.error === undefined) {
                    localStorage.setItem('accesstoken', promise.access_token);
                    localStorage.setItem('refresh_token', promise.refresh_token);
                    var timestamp = Math.round(new Date() / 1000) + promise.expires_in;
                    localStorage.setItem('session_expiresin', timestamp);
                    var session_expiresin = localStorage.getItem('session_expiresin');
                }
                //else {
                //    swal(promise.error_description);
                //    return;
                //}
                deferred.resolve(promise);
            })
            .error(function (promise) {
                deferred.resolve(promise);
            });
    };
    function getAccesstokenFromRefreshtoken(module, parameter) {
        var session_refresh_token = localStorage.getItem('refresh_token');
        console.log("hitting Service=============");
        var deferred = $q.defer();
        $http.post(apiBase + module, parameter,
            {
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;Authorization:Bearer ' + session_refresh_token }
            })
            .success(function (promise) {
                if (response.isSessionExpired == true) {
                    $cookieStore.remove('pagetitle');
                    $cookieStore.put("pagetitle", "Home");
                    $cookieStore.remove('IsLogged');
                    apiService.getURI("Login/clearsession", 1).
                        then(function (promise) {
                            //$state.go("login");
                            if (promise.subDomainNamelogout != null && promise.subDomainNamelogout != "") {
                                window.location.href = "http://localhost:57606/#/login/" + promise.subDomainNamelogout;
                            }
                            else {
                                window.location.href = "http://localhost:57606/#/login/";
                            }
                        })
                    swal("Your Session has been Expired.....Please Re-login");
                }
                deferred.resolve(promise);
                if (promise.error === undefined) {
                    var accesstoken = promise.access_token;
                    var token_type = promise.token_type;
                    var expires_in = promise.expires_in;
                    var refresh_token = promise.refresh_token;
                    localStorage.setItem('accesstoken', accesstoken);
                    localStorage.setItem('refresh_token', refresh_token);
                    var timestamp = Math.round(new Date() / 1000) + expires_in;
                    localStorage.setItem('session_expiresin', timestamp);
                    var session_expiresin = localStorage.getItem('session_expiresin');
                    //if (Math.round(new Date() / 1000) > timestamp) {
                    //    alert("Session has expired");
                    //}
                    // $state.go('app.homepage');
                }
                else {
                    swal(promise.error_description);
                    return;
                }
            })
            .error(function (promise) {
                deferred.resolve(promise);
            });
    };
    apiService.get = get;
    apiService.create = create;
    apiService.update = update;
    apiService.delet = delet;
    apiService.getURI = getURI;
    apiService.getDATA = getDATA;
    apiService.DeleteURI = DeleteURI;
    //  apiService.createAuth = createAuth;
    return apiService;
}]);
