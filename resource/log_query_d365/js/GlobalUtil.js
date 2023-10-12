if (!GlobalUtil) { GlobalUtil = { __namespace: true }; }

var GlobalUtil = {
    RequiredFieldArray: new Array(),
    EnabledFieldArray: new Array(),

    //重新加载表单
    reloadForm: function (formContext) {
        formContext.ui.formSelector.getCurrentItem().navigate();
    },

    //刷新子网格,subGridName为子网格名称
    refreshSubGrid: function (formContext, subGridName) {
        var subGridControl = formContext.getControl(subGridName);
        GlobalUtil.refreshSubGrid(subGridControl);
    },
    //刷新子网格,subGridControl为子网格控件
    refreshSubGrid: function (subGridControl) {
        if (subGridControl && typeof (subGridControl.refresh) == "function") subGridControl.refresh();
    },

    //获取字段的JQuery控件
    getFieldJQueryControl: function (fieldLogicalName) {
        var $field = parent.$("#" + fieldLogicalName);//jquery控件：审批人查询条件
        if (!$field || $field.length == 0) $field = parent.$(parent.$.find('div[data-id="' + fieldLogicalName + '"]')).find('textarea');
        //if (!$field || $field.length == 0) this.getFieldJQueryControl(fieldLogicalName);

        return $field;
    },

    //隐藏子网格 + 按钮
    hideSubGridButton: function (subgridName, targetWindow) {
        if (!targetWindow || !targetWindow.$) return;

        setTimeout(function () {
            debugger;
            var divSubGrid = targetWindow.$("#" + subgridName + "_contextualButtonsContainer");
            if (divSubGrid && divSubGrid.length > 0) {
                divSubGrid.hide();
            } else GlobalUtil.hideSubGridButton(subgridName, targetWindow.parent);
        }, 1000);
    },

    //打开Dynamics内置的模态窗口
    openDialog: function (url, dialogOptions, dialogArguments, initFunctionName, returnFunction) {
        if (Xrm.Internal.openLegacyWebDialog) { //V9.0
            Xrm.Internal.openLegacyWebDialog(url, dialogOptions, dialogArguments, initFunctionName, returnFunction);
        } else if (Xrm.Utility.openDialog) { //V8.0
            Xrm.Utility.openDialog(url, dialogOptions, dialogArguments, initFunctionName, returnFunction);
        } else if (Xrm.Internal.openDialog) { //V8.0
            Xrm.Internal.openDialog(url, dialogOptions, dialogArguments, initFunctionName, returnFunction);
        } else {
            this.openAlertDialog({ title: "Error", text: "Cannot open dialog,please contact with administrator" }); return
        }
    },

    //使用Dynamics内置的方法打开Alert框，兼容8.x与9.x
    openAlertDialog: function (alertStrings, alertOptions) {
        var callBack = new Promise(function (resolve, reject) {
            var blnIsUci = false;
            if (Xrm.Internal && Xrm.Internal.isUci && typeof (Xrm.Internal.isUci) == "function") blnIsUci = Xrm.Internal.isUci();

            if (!blnIsUci && parent.Xrm.Utility.alertDialog) {
                parent.Xrm.Utility.alertDialog(alertStrings.text, function () {
                    resolve();
                    return;
                });
            }
            else if (parent.Xrm.Navigation.openAlertDialog) {
                parent.Xrm.Navigation.openAlertDialog(alertStrings, alertOptions).then(function (result) {
                    resolve(result);
                    return;
                }, function (error) {
                    reject(error);
                    return;
                });
            }
        });
        return callBack;
    },

    //使用Dynamics内置的方法打开Confirm框，兼容8.x与9.x
    openConfirmDialog: function (confirmStrings, confirmOptions) {
        var callBack = new Promise(function (resolve, reject) {
            var blnIsUci = false;
            if (Xrm.Internal && Xrm.Internal.isUci && typeof (Xrm.Internal.isUci) == "function") blnIsUci = Xrm.Internal.isUci();

            if (!blnIsUci && parent.Xrm.Utility.confirmDialog) {
                var objResult = new Object();
                parent.Xrm.Utility.confirmDialog(confirmStrings.text, function () {
                    objResult.confirmed = true;
                    resolve(objResult);
                    return;
                }, function () {
                    objResult.confirmed = false;
                    resolve(objResult);
                    return;
                });
            }
            else if (parent.Xrm.Navigation.openConfirmDialog) {
                parent.Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(function (result) {
                    resolve(result);
                    return;
                }, function (error) {
                    reject(error);
                    return;
                });
            }
        });
        return callBack;
    },

    //获取WebApi操作异常的提示信息
    getWebApiErrorMessage: function (objError, defaultMessage) {
        var errorMessage = "";
        if (objError) {
            if (objError.message) {
                try {

                    var errorJson = JSON.parse(objError.message);
                    if (errorJson && errorJson.error) errorMessage = errorJson.error.message;
                    else errorMessage = objError.message;
                } catch (ex) {
                    errorMessage = objError.message
                }
            } else if (typeof (objError) == "string") errorMessage = objError;
        }

        if (!errorMessage) errorMessage = (defaultMessage && defaultMessage.replace(" ", "").length > 0) ? defaultMessage : "操作失败，发生异常";

        return errorMessage;
    },

    //禁用窗体上的所有字段
    disabledAllField: function (formContext) {
        var formControls = formContext.ui.controls;
        if (formControls && formControls.getLength() > 0) {
            for (var i = 0; i < formControls.getLength(); i++) {
                var singleControl = formControls.getByIndex(i);
                if (singleControl && singleControl.setDisabled) {
                    singleControl.setDisabled(true);
                }
            }
        }

        return true;
    },

    //设置窗体上单个字段是否可编辑
    setSingleFieldDisabled: function (formContext, strFieldLogicalNames, disabled) {
        formContext.ui.controls.getByName(strFieldLogicalNames).setDisabled(disabled);

        return true;
    },


    //禁用子网格上的字段，gridExecuteContext：子网格上下文，strFieldLogicalNames：需要禁用的字段列表（多个字段名以逗号分隔）
    /*如果在OnRecordSelect事件的配置页面中设置，则设置规则如下：
     * 库：GlobalUtil
     * 函数：GlobalUtil.disabledSubGridCells
     * 已启用：勾选
     * 将执行上下文作为第一个参数传递：勾选
     * 将传递给函数的以逗号分隔的参数列表："new_status,new_confirmedby"（示例，注意必须加上双引号）
     */
    disabledSubGridCells: function (gridExecuteContext, strFieldLogicalNames) {
        var aryFieldLogicalNames = [];
        if (strFieldLogicalNames) aryFieldLogicalNames = strFieldLogicalNames.split(',');

        var aryAllAttributes = gridExecuteContext.getFormContext().getData().getEntity().getAttributes();
        for (var i = 0; i < aryAllAttributes.getLength(); i++) {
            var objAttr = aryAllAttributes.getByIndex(i);
            if (!objAttr || !objAttr.controls || objAttr.controls.getLength() == 0) continue;

            if (!aryFieldLogicalNames || aryFieldLogicalNames.length == 0) {
                objAttr.controls.forEach(function (objControl) {
                    objControl.setDisabled(true);
                });
            } else {
                var attrName = objAttr.getName();
                var filterResult_FieldName = aryFieldLogicalNames.filter(function (fieldName) { return fieldName.toLowerCase() == attrName; });
                if (filterResult_FieldName.length > 0) {
                    objAttr.controls.forEach(function (objControl) {
                        objControl.setDisabled(true);
                    });
                }
            }
        }
    },

    //启用子网格上的字段，不在strFieldLogicalNames数组参数中的字段，将会被禁用，gridExecuteContext：子网格上下文，strFieldLogicalNames：需要启用的字段列表（多个字段名以逗号分隔）
    /*如果在OnRecordSelect事件的配置页面中设置，则设置规则如下：
     * 库：GlobalUtil
     * 函数：GlobalUtil.enabledSubGridCells
     * 已启用：勾选
     * 将执行上下文作为第一个参数传递：勾选
     * 将传递给函数的以逗号分隔的参数列表："new_status,new_confirmedby"（示例，注意必须加上双引号）
     */
    enabledSubGridCells: function (gridExecuteContext, strFieldLogicalNames) {
        debugger;
        var aryFieldLogicalNames = [];
        if (strFieldLogicalNames) aryFieldLogicalNames = strFieldLogicalNames.split(',');
        var aryAllAttributes = gridExecuteContext.getFormContext().getData().getEntity().getAttributes();
        for (var i = 0; i < aryAllAttributes.getLength(); i++) {
            var objAttr = aryAllAttributes.getByIndex(i);
            if (!objAttr || !objAttr.controls || objAttr.controls.getLength() == 0) continue;

            if (!aryFieldLogicalNames || aryFieldLogicalNames.length == 0) continue;

            var attrName = objAttr.getName();
            var filterResult_FieldName = aryFieldLogicalNames.filter(function (fieldName) { return fieldName.toLowerCase() == attrName; });
            if (filterResult_FieldName.length == 0) {
                objAttr.controls.forEach(function (objControl) {
                    objControl.setDisabled(true);
                });
            }
        }
    },

    //设置字段的可见性：①不可见时，隐藏、禁用字段，若isClearValue=true则同时清空字段的值 ②可见时，显示、启用字段
    setVisibleForField: function (formContext, fieldLogicalName, isVisible, isClearValue = true) {
        var fieldAttribute = formContext.getAttribute(fieldLogicalName);//字段属性
        var fieldControl = formContext.getControl(fieldLogicalName);//字段控件

        if (!isVisible) {
            if (!fieldControl.getDisabled()) {
                if (this.EnabledFieldArray.indexOf(fieldLogicalName) < 0) this.EnabledFieldArray.push(fieldLogicalName);
                fieldControl.setDisabled(true);
            }
            if (fieldAttribute.getRequiredLevel() == "required") {
                if (this.RequiredFieldArray.indexOf(fieldLogicalName) < 0) this.RequiredFieldArray.push(fieldLogicalName);
                fieldAttribute.setRequiredLevel("none");
            }

            fieldControl.setVisible(false)
            if (isClearValue) fieldAttribute.setValue(null);
        } else {
            if (this.EnabledFieldArray.indexOf(fieldLogicalName) >= 0) fieldControl.setDisabled(false);
            if (this.RequiredFieldArray.indexOf(fieldLogicalName) >= 0) fieldAttribute.setRequiredLevel("required");
            fieldControl.setVisible(true)
        }
    },

    //设置节的可见性：①不可见时，隐藏、禁用字段，若isClearValue=true则同时清空字段的值 ②可见时，显示、启用字段
    setVisibleForSection: function (formContext, tabName, sectionName, isVisible, isClearValue = true) {
        var tabControl = formContext.ui.tabs.get(tabName);
        if (!tabControl) return;
        var sectionControl = tabControl.sections.get(sectionName);
        if (!sectionControl || !sectionControl.controls || !sectionControl.controls.getLength || sectionControl.controls.getLength() == 0) return;

        sectionControl.setVisible(isVisible);
        for (var i = 0; i < sectionControl.controls.getLength(); i++) {
            var control = sectionControl.controls.get(i);
            this.setVisibleForField(formContext, control.getName(), isVisible, isClearValue);
        }
    },

    //设置选项卡的可见性：①不可见时，隐藏、禁用字段，若isClearValue=true则同时清空字段的值 ②可见时，显示、启用字段
    setVisibleForTab: function (formContext, tabName, isVisible, isClearValue = true) {
        var tabControl = formContext.ui.tabs.get(tabName);
        if (!tabControl || !tabControl.sections || !tabControl.sections.getLength || tabControl.sections.getLength() == 0) return;

        tabControl.setVisible(isVisible);
        for (var i = 0; i < tabControl.sections.getLength(); i++) {
            var section = tabControl.sections.get(i);
            this.setVisibleForSection(formContext, tabName, section.getName(), isVisible, isClearValue);
        }
    },

    //选项集字段：重新添加所有选项
    readdAllOptions: function (formContext, fieldLogicalName) {
        var fieldAttribute = formContext.getAttribute(fieldLogicalName);//字段属性
        var fieldControl = formContext.getControl(fieldLogicalName);//字段控件

        var allOptions = fieldAttribute.getOptions();
        fieldControl.clearOptions();

        for (var i = 0; i < allOptions.length; i++) {
            fieldControl.addOption(allOptions[i]);
        }
    },

    //选项集字段：根据value获取单个Option
    filterOptionByValue: function (allOptions, value) {
        if (!allOptions || allOptions.length == 0) return null;

        var filterResult = allOptions.filter(function (item) { return item.value == value; });
        if (filterResult && filterResult.length > 0) return filterResult[0];

        return null;
    },

    //校验用户是否包含指定的角色
    isUserHasRole: function (aryRoleName, fnCallBack, blnDefaultResult) {
        if (blnDefaultResult == undefined) blnDefaultResult = false;
        if (!aryRoleName || aryRoleName.length == 0) {
            if (fnCallBack) {
                fnCallBack(blnDefaultResult);
                return;
            } else return { hasRole: blnDefaultResult };
        }

        var aryUserRoleId = Xrm.Utility.getGlobalContext().userSettings.securityRoles;

        var filter_roleid = "";
        for (var i = 0; i < aryUserRoleId.length; i++) {
            var guidUserRoleId = GlobalUtil.trimBraceOfGuid(aryUserRoleId[i]);
            filter_roleid = filter_roleid + "roleid eq " + guidUserRoleId + "";
            if (i != aryUserRoleId.length - 1) filter_roleid += " or ";
        }
        var columnsOfTarget = "?$select=name&$filter=(" + filter_roleid + ")";//目标实体的查询参数
        if (fnCallBack) {
            top.Xrm.WebApi.retrieveMultipleRecords("role", columnsOfTarget).then(function (result) {
                if (result && result.entities && result.entities.length > 0) {
                    var aryMatchedRoleEntity = new Array();
                    for (var j = 0; j < result.entities.length; j++) {
                        var etn_role = result.entities[j];
                        var filteredRoleName = aryRoleName.filter(function (item) { return item == etn_role.name });
                        if (filteredRoleName && filteredRoleName.length > 0) aryMatchedRoleEntity.push(etn_role);
                    }
                    if (aryMatchedRoleEntity && aryMatchedRoleEntity.length > 0) fnCallBack(true, aryMatchedRoleEntity);
                    else fnCallBack(false);
                } else {
                    fnCallBack(false);
                }
            }, function (objError) {
                console.log(objError.message);
                fnCallBack(false);
            });
        }
        else {
            var result = GlobalUtil.WebApi.Sync.retrieve("/roles" + columnsOfTarget);
            if (result && result.value && result.value.length > 0) {
                var aryMatchedRoleEntity = new Array();
                for (var j = 0; j < result.value.length; j++) {
                    var etn_role = result.value[j];
                    var filteredRoleName = aryRoleName.filter(function (item) { return item == etn_role.name });
                    if (filteredRoleName && filteredRoleName.length > 0) aryMatchedRoleEntity.push(etn_role);
                }
                if (aryMatchedRoleEntity && aryMatchedRoleEntity.length > 0) return { hasRole: true, roleList: aryMatchedRoleEntity };
                else return { hasRole: false };
            } else {
                return { hasRole: false };
            }
        }
    },

    //校验用户是否隶属于指定业务部门
    isUserMemberOfBusinessUnits: function (aryBusinesUnitName, fnCallBack, blnDefaultResult) {
        if (blnDefaultResult == undefined) blnDefaultResult = false;
        if (!aryBusinesUnitName || aryBusinesUnitName.length == 0) {
            fnCallBack(blnDefaultResult);
            return;
        }

        var filter_bu_name = "";
        for (var i = 0; i < aryBusinesUnitName.length; i++) {
            filter_bu_name = filter_bu_name + "name eq '" + aryBusinesUnitName[i] + "'";
            if (i != aryBusinesUnitName.length - 1) filter_bu_name += " or ";
        }
        var columnsOfTarget = "?$select=systemuserid"
            + "&$expand=businessunitid($select=name;$filter=(" + filter_bu_name + "))";//目标业务实体的查询参数
        top.Xrm.WebApi.retrieveRecord("systemuser", Xrm.Utility.getGlobalContext().userSettings.userId, columnsOfTarget).then(function (result) {
            if (result && result.businessunitid) {
                fnCallBack(true, result.businessunitid);
            } else {
                fnCallBack(false);
            }
        }, function (objError) {
            console.log(objError.message);
            fnCallBack(false);
        });
    },

    //校验指定用户是否属于团队成员（传入团队Id数组）
    isTeamMemberById: function (aryTeamId, guidSystemUserId, fnCallBack, blnDefaultResult) {
        if (blnDefaultResult == undefined) blnDefaultResult = false;
        if (!aryTeamId || aryTeamId.length == 0) {
            fnCallBack(blnDefaultResult);
            return;
        }

        var filter_team_id = "";
        for (var i = 0; i < aryTeamId.length; i++) {
            filter_team_id = filter_team_id + "<condition attribute='teamid' operator='eq' value='" + aryTeamId[i] + "' />";
        }

        var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>"
            + "  <entity name='team'>"
            + "    <attribute name='name' />"
            + "    <attribute name='teamid' />"
            + "    <filter type='and'>"
            + "      <filter type='or'>"
            + filter_team_id
            + "      </filter>"
            + "    </filter>"
            + "    <link-entity name='teammembership' from='teamid' to='teamid' visible='false' intersect='true'>"
            + "      <link-entity name='systemuser' from='systemuserid' to='systemuserid' alias='ac'>"
            + "        <filter type='and'>"
            + "          <condition attribute='systemuserid' operator='eq' value='" + guidSystemUserId + "' />"
            + "        </filter>"
            + "      </link-entity>"
            + "    </link-entity>"
            + "  </entity>"
            + "</fetch>";
        if (fnCallBack) {
            GlobalUtil.WebApi.Async.get("/teams?fetchXml=" + encodeURIComponent(fetchXml)).then(
                function success(result) {
                    debugger;
                    if (result && result.value && result.value.length > 0) {
                        fnCallBack(true, result.value);
                    } else {
                        fnCallBack(false);
                    }
                },
                function fail(objError) {
                    fnCallBack(false, objError);
                }
            );
        }
        else {
            var result = GlobalUtil.WebApi.Sync.get("/teams?fetchXml=" + encodeURIComponent(fetchXml));
            if (result && result.success && result.data && result.data.value && result.data.value.length > 0) {
                return { isMember: true, teamList: result.data.value };
            } else {
                return { isMember: false };
            }
        }
    },

    //校验指定用户是否属于团队成员（传入团队名称数组）
    isTeamMemberByName: function (aryTeamName, guidSystemUserId, fnCallBack, blnDefaultResult) {
        if (blnDefaultResult == undefined) blnDefaultResult = false;
        if (!aryTeamName || aryTeamName.length == 0) {
            fnCallBack(blnDefaultResult);
            return;
        }

        var filter_team_name = "";
        for (var i = 0; i < aryTeamName.length; i++) {
            filter_team_name = filter_team_name + "<condition attribute='name' operator='eq' value='" + aryTeamName[i] + "' />";
        }

        var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>"
            + "  <entity name='team'>"
            + "    <attribute name='name' />"
            + "    <attribute name='teamid' />"
            + "    <filter type='and'>"
            + "      <filter type='or'>"
            + filter_team_name
            + "      </filter>"
            + "    </filter>"
            + "    <link-entity name='teammembership' from='teamid' to='teamid' visible='false' intersect='true'>"
            + "      <link-entity name='systemuser' from='systemuserid' to='systemuserid' alias='ac'>"
            + "        <filter type='and'>"
            + "          <condition attribute='systemuserid' operator='eq' value='" + guidSystemUserId + "' />"
            + "        </filter>"
            + "      </link-entity>"
            + "    </link-entity>"
            + "  </entity>"
            + "</fetch>";
        if (fnCallBack) {
            GlobalUtil.WebApi.Async.get("/teams?fetchXml=" + encodeURIComponent(fetchXml)).then(
                function success(result) {
                    debugger;
                    if (result && result.value && result.value.length > 0) {
                        fnCallBack(true, result.value);
                    } else {
                        fnCallBack(false);
                    }
                },
                function fail(objError) {
                    fnCallBack(false, objError);
                }
            );
        }
        else {
            var result = GlobalUtil.WebApi.Sync.get("/teams?fetchXml=" + encodeURIComponent(fetchXml));
            if (result && result.success && result.data && result.data.value && result.data.value.length > 0) {
                return { isMember: true, teamList: result.data.value };
            } else {
                return { isMember: false };
            }
        }
    },

    //获取配置参数的值
    //paramName为参数名；fnSuccess为查询成功时的回调函数，函数的参数为配置参数的值；defaultValue为未找到配置参数记录时的默认返回值
    getConfirmParamValue: function (paramName, fnSuccess, defaultValue = null) {
        var columnsOption = "?$select=new_configurationparameter_parametervalue&$filter=statecode eq 0 "
            + "and new_configurationparameter_parametername eq '" + paramName + "'"
        if (fnSuccess) {
            Xrm.WebApi.retrieveMultipleRecords("new_configurationparameter", columnsOption).then(function (objResult) {
                var paramValue = defaultValue;
                if (objResult && objResult.entities && objResult.entities.length > 0) {
                    var etnConfirm = objResult.entities[0];
                    paramValue = etnConfirm.new_configurationparameter_parametervalue;
                }
                fnSuccess(paramValue);
            }, function (objError) {
                GlobalUtil.openAlertDialog({ title: DialogTitle.error, text: GlobalUtil.getWebApiErrorMessage(objError) });
            });
        }
        else {
            var objResult = GlobalUtil.WebApi.Sync.retrieve("new_configurationparameters" + columnsOption);
            if (objResult && objResult.value && objResult.value.length > 0) {
                return objResult.value[0].new_configurationparameter_parametervalue;
            }

            return defaultValue;
        }
    },

    //根据Lookup字段自动带出对应实体的字段，并赋值到当前页面的对应字段
    //示例代码：var aryColumns = new Array();
    //          aryColumns.push({ source: "name", target: "ns_accountname" });//source为Lookup字段对应实体的字段，expand为用于查找Lookup实体的关联实体的名称，target为当前实体的字段
    //          aryColumns.push({ source: "_ns_paymenttermid_value", target: "ns_paymenttermid" });
    //          aryColumns.push({ source: "ns_settlemethod", target: "ns_settlemethod" });
    //          aryColumns.push({ source: "ns_signsite", expand: "ns_companyid", target: "ns_signsite" });
    //          GlobalUtil.autoSetValueByLookup(formContext, lookupFieldLogicalName, aryColumns, fnCallBack); //fnCallBack的参数为WebApi查询返回的原始对象
    autoSetValueByLookup: function (formContext, lookupFieldLogicalName, aryColumns, fnCallBack) {
        var lookupSourceFieldAttribute = formContext.getAttribute(lookupFieldLogicalName);//字段：客户编号

        var lookupSourceValue;
        if (lookupSourceFieldAttribute.getValue() && lookupSourceFieldAttribute.getValue().length > 0) lookupSourceValue = lookupSourceFieldAttribute.getValue()[0];

        this.autoSetValueByER(formContext, lookupSourceValue, aryColumns, fnCallBack);
    },

    //根据EntityReference对象自动带出对应实体的字段，并赋值到当前页面的对应字段
    //示例代码：
    //          var aryColumns = new Array();
    //          aryColumns.push({ source: "name", target: "ns_accountname" });//source为Lookup字段对应实体的字段，expand为用于查找Lookup实体的关联实体的名称，target为当前实体的字段
    //          aryColumns.push({ source: "_ns_paymenttermid_value", target: "ns_paymenttermid", isFireOnChange:true });//isFireOnChange为是否触发OnChange事件，默认为是
    //          aryColumns.push({ source: "ns_settlemethod", target: "ns_settlemethod",format:"text" });//format为针对选项集和查找类型字段赋值的格式，默认为value（value：将原始值直接赋值给目标字段，text：将原始值的显示文本赋值给目标字段）
    //          aryColumns.push({ source: "ns_signsite", expand: "ns_companyid", target: "ns_signsite" });
    //          GlobalUtil.autoSetValueByER(formContext, objEntityReference, aryColumns, fnCallBack); //fnCallBack的参数为WebApi查询返回的原始对象
    autoSetValueByER: function (formContext, objEntityReference, aryColumns, fnCallBack) {
        if (!aryColumns || aryColumns.length == 0) return;

        if (objEntityReference && objEntityReference.entityType && objEntityReference.id) {
            var selectFields = "";
            var expandEntities = new Array();

            //获取$select字段，并准备expand集合
            for (var i = 0; i < aryColumns.length; i++) {
                var objColumn = aryColumns[i];
                if (typeof (objColumn.isFireOnChange) == "undefined") objColumn.isFireOnChange = true;
                if (typeof (objColumn.format) == "undefined") objColumn.format = "value";

                if (objColumn.source) {
                    if (objColumn.expand && objColumn.expand.length > 0) {
                        var expandEntity = expandEntities.filter(function (item) { return item.name == objColumn.expand; });
                        if (!expandEntity || expandEntity.length == 0) {
                            expandEntity = { name: objColumn.expand, selectFields: "" };
                            expandEntities.push(expandEntity);
                        }
                        expandEntity.selectFields = expandEntity.selectFields + objColumn.source + ","
                    } else {
                        if (objColumn.target) {
                            var targetAttr = formContext.getAttribute(objColumn.target);
                            if (targetAttr && targetAttr.getAttributeType() == "lookup" && !objColumn.source.startsWith("_")) {
                                objColumn.source = "_" + objColumn.source + "_value";
                            }
                        }

                        selectFields = selectFields + objColumn.source + ",";
                    }
                }
            }

            var expandFields = "";
            //获取$expand的$select字段
            if (expandEntities && expandEntities.length > 0) {
                for (var j = 0; j < expandEntities.length; j++) {
                    var expandEntity = expandEntities[j];
                    expandFields = expandFields + expandEntity.name + "($top=1;$orderby=createdon desc;$select=" + expandEntity.selectFields.substr(0, expandEntity.selectFields.length - 1) + "),"
                }
            }
            var columnOption = "?$select=" + selectFields.substr(0, selectFields.length - 1) + "&$expand=" + expandFields.substr(0, expandFields.length - 1);
            Xrm.WebApi.retrieveRecord(objEntityReference.entityType, objEntityReference.id.replace("{", "").replace("}", ""), columnOption).then(function (objEntity) {
                if (objEntity) {
                    for (var i = 0; i < aryColumns.length; i++) {
                        var objColumn = aryColumns[i];
                        var fieldAttribute = formContext.getAttribute(objColumn.target);
                        if (!fieldAttribute) continue;

                        var objResultEntity;
                        if (objColumn.expand && objColumn.expand.length > 0 && typeof (objEntity[objColumn.expand]) == "object") {
                            if (objEntity[objColumn.expand] == null) {
                                fieldAttribute.setValue(null);
                                continue;
                            }
                            if (objEntity[objColumn.expand].length && objEntity[objColumn.expand].length == 1) {
                                objResultEntity = objEntity[objColumn.expand][0];
                            } else objResultEntity = objEntity[objColumn.expand];
                        } else objResultEntity = objEntity;

                        if (objResultEntity[objColumn.source]) {
                            switch (fieldAttribute.getAttributeType()) {
                                case "lookup":
                                    var erValue = GlobalUtil.getERFromWebApiResult(objResultEntity, objColumn.source);
                                    if (objColumn.format && objColumn.format.toLowerCase() == "text") fieldAttribute.setValue(erValue.name);
                                    else fieldAttribute.setValue([erValue]);
                                    break;
                                default:
                                    if (objColumn.format && objColumn.format.toLowerCase() == "text") {
                                        fieldAttribute.setValue(objResultEntity[objColumn.source + "@OData.Community.Display.V1.FormattedValue"]);
                                    }
                                    else {
                                        fieldAttribute.setValue(objResultEntity[objColumn.source]);
                                    }
                                    break;
                            }
                        } else fieldAttribute.setValue(null);

                        if (objColumn.isFireOnChange) fieldAttribute.fireOnChange();
                    }
                }
                if (fnCallBack && typeof (fnCallBack) == "function") fnCallBack(objEntity, objEntityReference);
            }, function (objError) {
                Xrm.Navigation.openAlertDialog({ title: "错误", text: GlobalUtil.getWebApiErrorMessage(objError) }); return
            });
        }
        else {
            for (var i = 0; i < aryColumns.length; i++) {
                var objColumn = aryColumns[i];
                if (typeof (objColumn.isFireOnChange) == "undefined") objColumn.isFireOnChange = true;

                var fieldAttribute = formContext.getAttribute(objColumn.target);
                if (!fieldAttribute) continue;
                fieldAttribute.setValue(null);
                if (objColumn.isFireOnChange) fieldAttribute.fireOnChange();
            }

            if (fnCallBack && typeof (fnCallBack) == "function") fnCallBack(null, objEntityReference);
        }
    },

    //获取WebApi查询结果中的lookup类型字段的值（EntityReference类型）
    getERFromWebApiResult: function (webApiResult, lookupFieldName) {
        var webApiColumnName = (lookupFieldName.indexOf("_value") > 0 && lookupFieldName.substr(0, 1) == "_") ? lookupFieldName : "_" + lookupFieldName + "_value";
        var lookupValue = { entityType: webApiResult[webApiColumnName + "@Microsoft.Dynamics.CRM.lookuplogicalname"], id: webApiResult[webApiColumnName], name: webApiResult[webApiColumnName + "@OData.Community.Display.V1.FormattedValue"] };
        return lookupValue;
    },

    //获取Action执行结果中的lookup类型字段的值（EntityReference类型）
    getERFromActionResult: function (actionResult) {
        var lookupValue;
        if (actionResult && actionResult["@odata.type"]) {
            var entityName = actionResult["@odata.type"].replace("#", "").replace("Microsoft.Dynamics.CRM.", "");
            lookupValue = { entityType: entityName, id: actionResult[entityName + "id"], name: "" };
        } else if (actionResult && actionResult["@odata.context"]) {
            var odataUrl = actionResult["@odata.context"];
            var intEntityNameStartIndex = odataUrl.indexOf("$metadata#") + 10;
            var intEntityNameEndIndex = odataUrl.indexOf("/", intEntityNameStartIndex) - 1;
            var entityName = odataUrl.substring(intEntityNameStartIndex, intEntityNameEndIndex);
            lookupValue = { entityType: entityName, id: GlobalUtil.trimBraceOfGuid(actionResult[entityName + "id"]), name: "" };
        }
        return lookupValue;
    },

    //获取服务器当前时间
    getDateNowFromServer: function () {
        try {
            return new Date($.ajax({ type: 'HEAD', async: false }).getResponseHeader("Date"));
        } catch (ex1) {
            try {
                var xmlHTTPRequest;
                if (window.ActiveXObject) xmlHTTPRequest = new ActiveXObject("Microsoft.XMLHTTP");
                else if (window.XMLHttpRequest) xmlHTTPRequest = new XMLHttpRequest();
                else xmlHTTPRequest = new ActiveXObject("Msxml2.XMLHTTP");
                xmlHTTPRequest.open('HEAD', '/?_=' + (-new Date), false);
                xmlHTTPRequest.send(null);

                return new Date(xmlHTTPRequest.getResponseHeader('Date'));
            } catch (ex2) {
                return new Date();
            }
        }
    },
    //从【配置参数】实体获取全局参数的值
    getGlobalParameterValue: function (aryNames, fnCallBack) {
        if (aryNames == null || aryNames.length == 0) return;

        var condition_new_parameter_name = "";
        for (var i = 0; i < aryNames.length; i++) {
            condition_new_parameter_name += "new_parameter_name eq '" + aryNames[i] + "'";
            if (i != aryNames.length - 1) condition_new_parameter_name += " or ";
        }
        var $filter = "?$select=new_parameter_name,new_parameter_value&$filter=statecode eq 0 and (" + condition_new_parameter_name + ")";
        Xrm.WebApi.retrieveMultipleRecords("new_configuration_parameters", $filter).then(function (objEntities) {
            var objResult = new Object();

            if (objEntities == null || objEntities.entities == null || objEntities.entities.length == 0) return fnCallBack(objResult);;

            for (var j = 0; j < objEntities.entities.length; j++) {
                var etnLoop = objEntities.entities[j];
                objResult[etnLoop["new_parameter_name"]] = etnLoop["new_parameter_value"];
            }

            fnCallBack(objResult);
        }, function (objError) {
            Xrm.Navigation.openAlertDialog({ title: "错误", text: GlobalUtil.getWebApiErrorMessage(objError) }); return;
        });
    },

    /**获取整数或小数类型字段的值
     * @param {object} formContext 窗体上下文
     * @param {string} attrName 字段逻辑名
     * @param {number} defaultValue 字段为空时返回的默认值，默认为0
     **/
    getNumberValue: function (formContext, attrName, defaultValue) {
        if (!defaultValue) defaultValue = 0;

        var objFieldAttr = formContext.getAttribute(attrName);
        if (objFieldAttr && objFieldAttr.getValue()) return objFieldAttr.getValue();
        else return defaultValue;
    },
    //获取Lookup字段的Value
    getERFieldValue: function (formContext, attrName) {
        var lookupAttr = formContext.getAttribute(attrName);
        if (lookupAttr && lookupAttr.getValue()) {
            var lookup = lookupAttr.getValue();
            if (lookup.length > 0) {
                var result = lookup[0];
                var ent = {};
                ent["name"] = result["name"];
                ent["id"] = GlobalUtil.trimBraceOfGuid(result.id);
                ent["entityType"] = result["entityType"];
                ent["type"] = result["entityType"];
                return ent;
            }
        }

        return null;
    },
    //获取lookup字段的id,去除花括号，小写
    getERFieldId: function (formContext, attrName) {
        var lookupValue = GlobalUtil.getERFieldValue(formContext, attrName);
        if (lookupValue) {
            return lookupValue.id;
        }
        return null;
    },
    //给lookup字段赋值
    setERFieldValue: function (formContext, attrName, entityId, entityLogicalName, entityPrimaryValue) {
        var lookupAttr = formContext.getAttribute(attrName);
        var lookup =
        {
            id: entityId,
            entityType: entityLogicalName,
            name: entityPrimaryValue
        };
        lookupAttr.setValue([lookup]);
    },
    //给lookup字段赋值-从webapi返回的结果记录中某一个lookup字段
    setERFieldFromWebApiResult: function (formContext, attrName, webApiResultEntity) {
        var lookupAttr = formContext.getAttribute(attrName);
        var lookup =
        {
            id: webApiResultEntity.attrName,
            entityType: webApiResultEntity[attrName + "@Microsoft.Dynamics.CRM.lookuplogicalname"],
            name: webApiResultEntity[attrName + "@OData.Community.Display.V1.FormattedValue"]
        };
        lookupAttr.setValue([lookup]);
    },
    /**
     * 从WebApi结果中获取复合数据类型的文本，如选项集、查找类型的显示文本
     * @param {any} attrName 字段逻辑名
     * @param {any} webApiResultEntity WebApi结果对象
     */
    getFormattedValueFromWebApiResult: function (attrName, webApiResultEntity) {
        return webApiResultEntity[attrName + "@OData.Community.Display.V1.FormattedValue"];
    },
    //清空字段
    clearFieldValue: function (formContext, attrName, isFireOnChange) {
        var attr = formContext.getAttribute(attrName);
        if (attr) {
            attr.setValue(null);
            if (isFireOnChange) attr.fireOnChange();
        }
    },
    //去除字符串的花括号并转为小写
    trimBraceOfGuid: function (id) {
        if (id) {
            id = id.replace("{", "").replace("}", "").toLowerCase();
        }
        return id;
    },

    /** 比较2个变量的值是否相同
     **/
    isEqual: function (value1, value2) {
        if (value1 == null && value2 == null) return true;
        if (value1 == null && value2 != null) return false;
        if (value1 != null && value2 == null) return false;

        var valueType = typeof (value1);

        if (valueType == "number") return value1 == value2;
        else if (valueType == "string") {
            return value1.toLowerCase() == value2.toLowerCase();
        }

        return false;
    },

    load: function () { },

    //经典模式下的方法
    Classic: {
        //校验用户是否包含指定的角色
        isUserHasRole: function (roleNames) {
            if (!roleNames) return false;
            var aryCheckRoleName = roleNames.split(',');//安全角色
            var result = GlobalUtil.isUserHasRole(aryCheckRoleName);
            return result.hasRole;
        }
    },
    WebApi: {
        //获取客户端URL（仅包含主机名/域名、组织名）
        getClientUrl: function () {
            var context;
            // GetGlobalContext defined by including reference to   
            // ClientGlobalContext.js.aspx in the HTML page.  
            if (typeof GetGlobalContext != "undefined") {
                context = GetGlobalContext();
            } else {
                if (typeof Xrm != "undefined") {
                    // Xrm.Page.context defined within the Xrm.Page object model for form scripts.  
                    context = Xrm.Page.context;
                } else {
                    throw new Error("Context is not available.");
                }
            }
            return context.getClientUrl();
        },
        //获取客户端WebApi URL
        getWebApiUrl: function () {
            return this.getClientUrl() + "/api/data/v9.2";
        },
        //根据实体逻辑名获取实体复数名
        getEntityCollectionName: function (entityLogicalName) {
            var entityCollectionName = "";
            if (entityLogicalName && entityLogicalName.length > 0) {
                var logicalNameExceptEndLetter = entityLogicalName.substr(0, entityLogicalName.length - 1);
                var logicalNameEndLetter = entityLogicalName.substr(entityLogicalName.length - 1, 1);
                if (logicalNameEndLetter.toLowerCase() == "y") entityCollectionName = logicalNameExceptEndLetter + "ies";
                else if (logicalNameEndLetter.toLowerCase() == "s" || logicalNameEndLetter.toLowerCase() == "x") entityCollectionName = entityLogicalName + "es";

                logicalNameExceptEndLetter = entityLogicalName.substr(0, entityLogicalName.length - 2);
                logicalNameEndLetter = entityLogicalName.substr(entityLogicalName.length - 2, 2);
                if (logicalNameEndLetter.toLowerCase() == "ch" || logicalNameEndLetter.toLowerCase() == "sh") entityCollectionName = entityLogicalName + "es";

                if (!entityCollectionName || entityCollectionName.length == 0) entityCollectionName = entityLogicalName + "s";
            }
            return entityCollectionName;
        },
        //请求WebApi的方式
        MethodName: {
            POST: "POST",
            PATCH: "PATCH",
            PUT: "PUT",
            GET: "GET",
            DELETE: "DELETE"
        },
        Async: {
            //执行Action、Function
            execute: function (method, uri, data, headers) {
                if (!RegExp(method, "g").test("POST PATCH PUT GET DELETE")) { // Expected action verbs.  
                    throw new Error("GlobalUtil.WebApi.Async.execute: method parameter must be one of the following: " +
                        "POST, PATCH, PUT, GET, or DELETE.");
                }
                if (!typeof uri === "string") {
                    throw new Error("GlobalUtil.WebApi.Async.execute: uri parameter must be a string.");
                }
                if ((RegExp(method, "g").test("PATCH PUT")) && (!data)) {
                    throw new Error("GlobalUtil.WebApi.Async.execute: data parameter must not be null for operations that create or modify data.");
                }

                // Construct a fully qualified URI if a relative URI is passed in.  
                if (uri.charAt(0) === "/") {
                    uri = GlobalUtil.WebApi.getWebApiUrl() + uri;
                }

                return new Promise(function (resolve, reject) {
                    var request = new XMLHttpRequest();
                    request.open(method, encodeURI(uri), true);
                    request.setRequestHeader("OData-MaxVersion", "4.0");
                    request.setRequestHeader("OData-Version", "4.0");
                    request.setRequestHeader("Accept", "application/json");
                    request.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                    if (headers && headers.length > 0) {
                        for (var i = 0; i < headers.length; i++) {
                            var addHeader = headers[i];
                            if (addHeader) {
                                if (typeof addHeader.name != "string" || typeof addHeader.value != "string") {
                                    throw new Error("GlobalUtil.WebApi.Async.execute: headers parameter must have name and value properties that are strings.");
                                }
                            }
                            request.setRequestHeader(addHeader.name, addHeader.value);
                        }

                    }
                    request.onreadystatechange = function () {
                        if (this.readyState === 4) {
                            request.onreadystatechange = null;
                            switch (this.status) {
                                case 200: // Success with content returned in response body.  
                                case 204: // Success with no content returned in response body.  
                                case 304: // Success with Not Modified 
                                    resolve(this);//this=request
                                    break;
                                default: // All other statuses are error cases.  
                                    var error;
                                    try {
                                        error = JSON.parse(request.response).error;
                                    } catch (e) {
                                        error = new Error("Unexpected Error");
                                    }
                                    reject(error);
                                    break;
                            }
                        }
                    };
                    if (data) request.send(JSON.stringify(data));
                    else request.send();
                });
            },

            //GET请求
            get: function (uri, headers) {
                return new Promise(function (resolve, reject) {
                    GlobalUtil.WebApi.Async.execute(GlobalUtil.WebApi.MethodName.GET, uri, null, headers).then(function (request) {
                        var jsonResponse;
                        if (request.response && typeof (request.response) == "string") jsonResponse = JSON.parse(request.response);
                        resolve(jsonResponse);
                    }, function (error) {
                        reject(error);
                    });
                });
            },

            //POST请求
            post: function (uri, data, headers) {
                return new Promise(function (resolve, reject) {
                    GlobalUtil.WebApi.Async.execute(GlobalUtil.WebApi.MethodName.POST, uri, data, headers).then(function (request) {
                        var jsonResponse;
                        if (request.response && typeof (request.response) == "string") jsonResponse = JSON.parse(request.response);
                        resolve(jsonResponse);
                    }, function (error) {
                        reject(error);
                    });
                });
            },

            //以SYSTEM权限执行fetchxml查询
            retrieveMultipleByAdmin: function (fetchXml, headers) {
                var actionParam = {
                    FetchXml: fetchXml
                };
                if (!headers) headers = [];
                headers.push({ name: "Prefer", value: "odata.include-annotations=\"*\"" });



                return new Promise(function (resolve, reject) {
                    GlobalUtil.WebApi.Async.execute(GlobalUtil.WebApi.MethodName.POST, "/new_QueryByAdmin", actionParam, headers).then(function (request) {
                        var objResult = { success: false, data: null };
                        objResult.success = true;
                        if (request.response && typeof (request.response) == "string") objResult.data = JSON.parse(request.response);
                        else if (request.status == 204 && request.response == "") {
                            var entityId = request.getResponseHeader("OData-EntityId");
                            if (entityId) objResult.data = JSON.parse("{ \"id\":\"" + request.getResponseHeader("OData-EntityId").match(/([^\(\)]+)(?=\))/g)[0] + "\" }");
                        }
                        else objResult.data = request.response;

                        if (objResult && objResult.data && objResult.data.value && objResult.data.value.length > 0)
                            resolve(objResult.data.value);
                        else resolve(null);
                    }, function (error) {
                        reject(error);
                    })
                });
            },
            //以SYSTEM权限执行fetchxml查询
            retrieveSingleByAdmin: function (fetchXml, headers) {
                return new Promise(function (resolve, reject) {
                    GlobalUtil.WebApi.Async.retrieveMultipleByAdmin(fetchXml, headers).then(
                        function (result) {
                            if (result && result.length > 0) resolve(result[0]);
                            else resolve(null);
                        }, function (error) {
                            reject(error);
                        }
                    );
                });
            }

        },

        Sync: {
            //执行同步请求
            execute: function (method, uri, data, headers) {
                if (!RegExp(method, "g").test("POST PATCH PUT GET DELETE")) { // Expected action verbs.  
                    throw new Error("GlobalUtil.WebApi.Sync.execute: method parameter must be one of the following: " +
                        "POST, PATCH, PUT, GET, or DELETE.");
                }
                if (!typeof uri === "string") {
                    throw new Error("GlobalUtil.WebApi.Sync.execute: uri parameter must be a string.");
                }
                if ((RegExp(method, "g").test("POST PATCH PUT")) && (!data)) {
                    throw new Error("GlobalUtil.WebApi.Sync.execute: data parameter must not be null for operations that create or modify data.");
                }


                // Construct a fully qualified URI if a relative URI is passed in.  
                if (uri.charAt(0) === "/") {
                    uri = GlobalUtil.WebApi.getWebApiUrl() + uri;
                }

                var request = new XMLHttpRequest();
                request.open(method, encodeURI(uri), false);
                request.setRequestHeader("OData-MaxVersion", "4.0");
                request.setRequestHeader("OData-Version", "4.0");
                request.setRequestHeader("Accept", "application/json");
                request.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                if (headers && headers.length > 0) {
                    for (var i = 0; i < headers.length; i++) {
                        var addHeader = headers[i];
                        if (addHeader) {
                            if (typeof addHeader.name != "string" || typeof addHeader.value != "string") {
                                throw new Error("GlobalUtil.WebApi.Sync.execute: headers parameter must have name and value properties that are strings.");
                            }
                        }
                        request.setRequestHeader(addHeader.name, addHeader.value);
                    }

                }

                if (data) request.send(JSON.stringify(data));
                else request.send();

                var objResult = { success: false, data: null };
                switch (request.status) {
                    case 200: // Success with content returned in response body.  
                    case 204: // Success with no content returned in response body.  
                    case 304: // Success with Not Modified 
                        objResult.success = true;
                        if (request.response && typeof (request.response) == "string") objResult.data = JSON.parse(request.response);
                        else if (request.status == 204 && request.response == "") {
                            var entityId = request.getResponseHeader("OData-EntityId");
                            if (entityId) objResult.data = JSON.parse("{ \"id\":\"" + request.getResponseHeader("OData-EntityId").match(/([^\(\)]+)(?=\))/g)[0] + "\" }");
                        }
                        else objResult.data = request.response;
                        break;
                    default: // All other statuses are error cases.  
                        var error;
                        try {
                            error = JSON.parse(request.response).error;
                        } catch (e) {
                            error = new Error("Unexpected Error");
                        }
                        objResult.data = error;
                        break;
                }

                return objResult;
            },

            //GET请求
            get: function (uri, headers) {
                return GlobalUtil.WebApi.Sync.execute(GlobalUtil.WebApi.MethodName.GET, uri, null, headers);
            },

            //POST请求
            post: function (uri, data, headers) {
                return GlobalUtil.WebApi.Sync.execute(GlobalUtil.WebApi.MethodName.POST, uri, data, headers);
            },

            //查询实体数据
            retrieve: function (parameter, headers) {
                var uri = parameter.charAt(0) === "/" ? parameter : "/" + parameter;
                if (!headers) headers = [];
                headers.push({ name: "Prefer", value: "odata.include-annotations=\"*\"" });
                var objResult = GlobalUtil.WebApi.Sync.get(uri, headers);

                if (objResult.success) {
                    return objResult.data;
                } else {
                    GlobalUtil.openAlertDialog({ title: DialogTitle.Error, text: GlobalUtil.getWebApiErrorMessage(objResult.data) });
                    return null;
                }
            },

            //根据实体记录主键查询数据（以当前登录用户权限执行）
            retrieveById: function (logicalName, id, columns, collectionName, headers) {
                if (!collectionName || collectionName.length == 0) {
                    collectionName = GlobalUtil.WebApi.getEntityCollectionName(logicalName);
                }
                var parameters = collectionName + "(" + id + ")?";
                if (columns && columns.length > 0) parameters = parameters + "$select=" + columns;
                return GlobalUtil.WebApi.Sync.retrieve(parameters, headers);
            },
            //以SYSTEM权限执行fetchxml查询
            retrieveMultipleByAdmin: function (fetchXml, headers) {
                var actionParam = {
                    FetchXml: fetchXml
                };
                if (!headers) headers = [];
                headers.push({ name: "Prefer", value: "odata.include-annotations=\"*\"" });
                var result = GlobalUtil.WebApi.Sync.post("/new_QueryByAdmin", actionParam, headers);
                if (result && result.data && result.data.value && result.data.value.length > 0) return result.data.value;

                return null;
            },
            //以SYSTEM权限执行fetchxml查询
            retrieveSingleByAdmin: function (fetchXml, headers) {
                var result = GlobalUtil.WebApi.Sync.retrieveMultipleByAdmin(fetchXml, headers);
                if (result && result.length > 0) return result[0];

                return null;
            },

            //使用fetchxml聚合运算计算count
            countByFetchXml: function (logicalName, strFetchXml) {
                var intTotalCount = 0;

                var result = GlobalUtil.WebApi.Sync.retrieve(GlobalUtil.WebApi.getEntityCollectionName(logicalName) + "?fetchXml=" + strFetchXml);
                if (result && result.value && result.value.length > 0) {
                    intTotalCount = result.value[0].TotalCount;
                }
                return intTotalCount;
            },
            //根据where条件查询count结果，where条件可以包含filter和linkentity,count结果保存在属性TotalCount中
            countByWhere: function (logicalName, strWhere) {
                if (!strWhere) strWhere = "";
                var strFetchXml = "<fetch mapping='logical' output-format='xml-platform' version='1.0' aggregate='true'>  "
                    + "  <entity name='" + logicalName + "'>  "
                    + "    <attribute name='" + logicalName + "id' alias='TotalCount' aggregate='count' />"
                    + strWhere
                    + "  </entity>  "
                    + "</fetch>";
                var result = GlobalUtil.WebApi.Sync.countByFetchXml(logicalName, strFetchXml);
                return result;
            },

            create: function (entityCollectionName, entityObj) {
                var objResult = GlobalUtil.WebApi.Sync.post("/" + entityCollectionName, entityObj);

                if (objResult.success) {
                    return objResult.data;
                } else {
                    GlobalUtil.openAlertDialog({ title: DialogTitle.Error, text: GlobalUtil.getWebApiErrorMessage(objResult.data) });
                    return null;
                }
            },
            updateByImpersonationUser: function (entityCollectionName, entityGuid, entityObj, entityField, MSCRMCallerID) {
                if (entityField == null || typeof (entityField) == "undefined") entityField = "";
                else entityField = "/" + entityField.replace(/^\//, "");

                var uri = "/" + entityCollectionName + "(" + entityGuid + ")" + entityField;
                var addHeader = null;
                if (MSCRMCallerID && MSCRMCallerID.length == 36) addHeader = { header: "MSCRMCallerID", value: MSCRMCallerID };

                var objResult = GlobalUtil.WebApi.Sync.execute(GlobalUtil.WebApi.MethodName.PATCH, uri, entityObj, addHeader);

                if (objResult.success) {
                    return objResult.data;
                } else {
                    GlobalUtil.openAlertDialog({ title: DialogTitle.Error, text: GlobalUtil.getWebApiErrorMessage(objResult.data) });
                    return null;
                }
            },
            update: function (entityCollectionName, entityGuid, entityObj, entityField) {
                return GlobalUtil.WebApi.Sync.updateByImpersonationUser(entityCollectionName, entityGuid, entityObj, entityField, null);
            },
            delete: function (entityCollectionName, entityGuid, entityField) {
                if (typeof (entityField) == "undefined") entityField = "";
                else entityField = "/" + entityField.replace(/^\//, "");

                var uri = "/" + entityCollectionName + "(" + entityGuid + ")" + entityField;
                var objResult = GlobalUtil.WebApi.Sync.execute(GlobalUtil.WebApi.MethodName.DELETE, uri);

                if (objResult.success) {
                    return objResult.data;
                } else {
                    GlobalUtil.openAlertDialog({ title: DialogTitle.Error, text: GlobalUtil.getWebApiErrorMessage(objResult.data) });
                    return null;
                }
            },
            execAction: function (actionName, entityObj) {
                var uri = actionName.charAt(0) === "/" ? actionName : "/" + actionName;
                var objResult = GlobalUtil.WebApi.Sync.post(uri, entityObj);
                if (objResult.success) {
                    return objResult.data;
                } else {
                    GlobalUtil.openAlertDialog({ title: DialogTitle.Error, text: GlobalUtil.getWebApiErrorMessage(objResult.data) });
                    return null;
                }
            }
        }

    }
};

Date.prototype.add = function (strInterval, addNum) {
    var dtTmp = new Date(this);
    if (isNaN(dtTmp)) return false;
    switch (strInterval) {
        case "s": return new Date(Date.parse(dtTmp) + (1000 * addNum));
        case "n": return new Date(Date.parse(dtTmp) + (60000 * addNum));
        case "h": return new Date(Date.parse(dtTmp) + (3600000 * addNum));
        case "d": return new Date(Date.parse(dtTmp) + (86400000 * addNum));
        case "w": return new Date(Date.parse(dtTmp) + ((86400000 * 7) * addNum));
        case "m": return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + addNum, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case "y": return new Date((dtTmp.getFullYear() + addNum), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
    }
    return false;
}

//CRM页面控件的事件名称
var CrmControlEventNames = {
    OnLoad: "OnLoad",
    OnChange: "OnChange"
}

/// <summary>
/// 公共参数：操作类型
/// </summary>
var Global_OperateType = {
    /// <summary>
    /// 无效的项目
    /// </summary>
    InvalidItem: -1,

    /// <summary>
    /// 提交
    /// </summary>
    Submit: 10
}

//弹出框执行结果
var DialogResult = {
    OK: 10, //完成
    Cancel: 20 //取消
}

//弹出框的Title
var DialogTitle = {
    Prompt: "提示",
    Warning: "警告",
    Error: "错误"
}

//为按钮引用该JS库
function ribbonRef() { }