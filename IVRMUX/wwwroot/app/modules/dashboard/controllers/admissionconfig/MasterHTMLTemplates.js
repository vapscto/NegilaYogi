
(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterHTMLTemplatesController', MasterHTMLTemplatesController)

    MasterHTMLTemplatesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'superCache', '$compile']
    function MasterHTMLTemplatesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, superCache, $compile) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.sortKey = 'imcC_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //TO  GEt The Values iN Grid
        //$scope.html = "<h1>Editor</h1>\n\n" +
        //    "<p>Note that 'change' is triggered when the editor loses focus.\n" +
        //    "<br /> That's when the Angular scope gets updated.</p>";
        $scope.datass = "";

        $scope.htmldata1 = '';
        $scope.viewemailtempate = function (dd) {
            
            $scope.htmldata1 = dd;

            var e1 = angular.element(document.getElementById("test"));
            $compile(e1.html($scope.htmldata1))(($scope));
        }

        $scope.BindData = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            apiService.getDATA("MasterSmsEmailParameter/htmlGetdetails").then(function (promise) {
              
                $scope.parameterlist = promise.parameterlist;
                $scope.data = promise.parms;

                $scope.html ="";

                 $("#editor").kendoEditor({
                    tools: [
                        "bold",
                        "italic",
                        "underline",
                        "strikethrough",
                        "justifyLeft",
                        "justifyCenter",
                        "justifyRight",
                        "justifyFull",
                        "insertUnorderedList",
                        "insertOrderedList",
                        "indent",
                        "outdent",
                        "createLink",
                        "unlink",
                        "insertImage",
                        "insertFile",
                        "subscript",
                        "superscript",
                        "tableWizard",
                        "createTable",
                        "addRowAbove",
                        "addRowBelow",
                        "addColumnLeft",
                        "addColumnRight",
                        "deleteRow",
                        "deleteColumn",
                        "mergeCellsHorizontally",
                        "mergeCellsVertically",
                        "splitCellHorizontally",
                        "splitCellVertically",
                        "formatting",
                        "cleanFormatting",
                        "copyFormat",
                        "applyFormat",
                        "fontName",
                        "fontSize",
                        "foreColor",
                        "backColor",
                        "print",
                        "viewHtml"
                     ],

                  


                     paste: onPaste
                });
                var editor = $("#editor").data("kendoEditor");

                editor.value($scope.html);
                editor.bind("change", editor_change);

                
               
            });
        };


        function onPaste(e) {
            kendo.htmlEncode(e.html);
        }
        //$scope.abcd = function () {
        //    debugger;
        //    $scope.html = document.getElementById("editor").editor;
        //}
        function editor_change() {
            $scope.datass = this.value();
            document.getElementById("deff").value = $scope.datass;
            
        }
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        //--Active Deactive---//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.ismhtmL_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {

                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterSmsEmailParameter/htmldeletedata/", user).
                            then(function (promise) {
                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnVal == true) {
                                        swal(confirmmgs + " " + "Successfully.");
                                        $state.reload();
                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");
                                        $state.reload();
                                    }
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $state.reload();
                });
        }



        //delete record
        $scope.Deletecastecategorydata = function (DeleteRecord, SweetAlert) {
            // swal(id);
            $scope.deleteId = DeleteRecord.ismP_ID;
            var MdeleteId = $scope.deleteId;
            swal({
                title: "Are You Sure?",
                text: "Do You Want To Delete The Record ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("MasterSmsEmailParameter/htmldeletedata", MdeleteId).then(function (promise) {
                            if (promise.message === "Delete") {
                                swal("You Can Not Delete This Record It Is Already Mapped With Student");
                            }
                            else {
                                swal('Record Deleted Successfully');
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        };

        // to Edit Data
        $scope.Editcastecategorydata = function (EditRecord) {

          
            apiService.create("MasterSmsEmailParameter/htmledit/", EditRecord).then(function (promise) {
                $scope.ISMHTML_Id = promise.editlist[0].ismhtmL_Id;
                $scope.ISMHTML_HTMLName = promise.editlist[0].ismhtmL_HTMLName;
                $scope.datass = promise.editlist[0].ismhtmL_HTMLTemplate;


                var editor = $("#editor").data("kendoEditor");

                editor.value($scope.datass);
               
            });
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        $scope.html = "";
        // TO Save The Data
        $scope.submitted = false;
        $scope.savedata = function () {
          
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMHTML_Id": $scope.ISMHTML_Id,
                    "ISMHTML_HTMLName": $scope.ISMHTML_HTMLName,
                    "ISMHTML_HTMLTemplate": $scope.datass,
                    
                };

                apiService.create("MasterSmsEmailParameter/htmlSavedata", data).then(function (promise) {
                    if (promise.message !== null && promise.message !== "") {
                        swal(promise.message);
                        $state.reload();
                        return;
                    }

                    if (promise.returnVal === true) {
                        if (promise.messageupdate === "Save") {
                            swal("Record Saved Successfully");
                        }
                        else if (promise.messageupdate === "Update") {
                            swal("Record Updated Successfully");
                        }
                        $state.reload();
                    }
                    else {
                        if (promise.messageupdate === "Save") {
                            swal("Record Failed To Save");
                        }
                        else if (promise.messageupdate === "Update") {
                            swal("Record Failed To Update");
                        }
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.searchValue = '';
      
    }
    angular
        .module('app').directive('txtArea', function () {
            return {
                restrict: 'AE',
                replace: 'true',
                scope: { data: '=', model: '=ngModel' },
                template: "<textarea></textarea>",
                link: function (scope, elem) {
                    scope.$watch('data', function (newVal) {
                        if (newVal) {
              
                            scope.model += newVal[0];
                            var editor = $("#editor").data("kendoEditor");

                            editor.value(scope.model);

                        }
                    });
                }
            };
        });

})();