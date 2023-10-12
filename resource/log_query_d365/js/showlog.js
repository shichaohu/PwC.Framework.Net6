"use strict";

var pwclog = window.pwclog || {};
(function () {
    let self = pwclog;
    this.apiInfo = {};
    this.writeErrorMessage = function (msg) {
        document.getElementById("msg").innerHTML = msg;
    }
    this.cleanLogResult = function () {
        document.getElementById("logBody").innerHTML = "";
        document.getElementById("msg").innerHTML = "";
    }

    this.displayLoading = function () {
        let loading = document.getElementById("loading");
        if (loading) loading.style = "";
    }

    this.hideLoading = function () {
        let loading = document.getElementById("loading");
        if (loading) loading.style.display = "none";
    }
    this.getApiName = function (apiString) {
        apiString = apiString.toLowerCase();
        for (var i = 0; i < self.apiInfo.length; i++) {
            var text = `${self.apiInfo[i].ControllerSummary}-${self.apiInfo[i].ActionSummary}`;
            var value = `${self.apiInfo[i].Controller}/${self.apiInfo[i].ActionRouteTemplate}`.toLowerCase();

            if (apiString.indexOf(value) > -1) {
                return text;
            }
        }
        return "";
    }
    this.loadApiSwaggerDocs = function () {
        //若不进行设置，其他引用js会报错
        window.Xrm = parent.Xrm;
        pwc.get("/api/Common/getallapiInfo").then(
            res => {
                console.log(res);
                document.getElementById("HttpPath").options.add(
                    new Option("全部", "")
                );
                if (res && res.Code == 200 && res.Value.length > 0) {
                    self.apiInfo = res.Value;
                    for (var i = 0; i < res.Value.length; i++) {

                        var text = `${res.Value[i].ControllerSummary}-${res.Value[i].ActionSummary}`;
                        var value = `${res.Value[i].Controller}/${res.Value[i].ActionRouteTemplate}`;
                        document.getElementById("HttpPath").options.add(
                            new Option(text, value)
                        );

                    }
                }
            },
            fail => {
                console.error(fail);
                this.writeErrorMessage(JSON.stringify(fail));
            }
        ).catch(error => {
            console.error(error);
            this.writeErrorMessage(error);
        })
    };
    this.getSelectValue = function (elementId) {
        var elementObj = document.getElementById(elementId);
        var elementSelIndex = elementObj.selectedIndex; // 选中索引
        var elementValue = elementObj.options[elementSelIndex].value; // 选中值
        return elementValue;
    }
    this.getSelectText = function (elementId) {
        var elementObj = document.getElementById(elementId);
        var elementSelIndex = elementObj.selectedIndex; // 选中索引
        var elementText = elementObj.options[elementSelIndex].text; // 选中文本
        return elementText;
    }
    this.loadData = function () {
        this.cleanLogResult();
        this.displayLoading();
        let btnSubmit = document.getElementById("btnSubmit");
        if (btnSubmit) btnSubmit.style = "";

        try {
            //若不进行设置，其他引用js会报错
            window.Xrm = parent.Xrm;

            var httpPathText = this.getSelectText("HttpPath");
            let params = {
                "HttpHost": "",//document.getElementById("HttpHost").value,
                "HttpPath": this.getSelectValue("HttpPath"),
                "HttpRequestId": document.getElementById("HttpRequestId").value,
                "Level": this.getSelectValue("Level"),
                "SourceContext": "",//document.getElementById("SourceContext").value,
                "Message": document.getElementById("Message").value,
                "TimeStart": document.getElementById("TimeStart").value,
                "TimeEnd": document.getElementById("TimeEnd").value,
                "Limit": document.getElementById("Limit").value
            };
            console.info(params);

            if (params) {
                pwc.post("/api/LogOperations/QueryDBLogs", params).then(
                    res => {
                        console.log(res);

                        if (res && res.Code == 200 && res.Value.length > 0) {
                            var trString = "";
                            for (var i = 0; i < res.Value.length; i++) {
                                var nowHttpPathText = "";
                                if (httpPathText == "全部") {
                                    nowHttpPathText = self.getApiName(res.Value[i].HttpPath);
                                }
                                else {
                                    nowHttpPathText = httpPathText;
                                }
                                trString += `<tr>
                                                    <td>${res.Value[i].Id}</td>
                                                    <td>${res.Value[i].HttpRemoteAddress}</td>
                                                    <td>${nowHttpPathText}</td>
                                                    <td>${res.Value[i].HttpPath}</td>
                                                    <td>${res.Value[i].HttpRequestId}</td>
                                                    <td>${res.Value[i].Timestamp}</td>
                                                    <td>${res.Value[i].Level}</td>
                                                    <td>${res.Value[i].Message}</td>
                                                </tr>
                                `
                            };
                            document.getElementById("logBody").innerHTML = trString;

                        } else {
                            this.writeErrorMessage(JSON.stringify(res));
                        }
                    },
                    fail => {
                        console.error(fail);
                        this.writeErrorMessage(JSON.stringify(fail));
                    }
                ).catch(error => {
                    console.error(error);
                    this.writeErrorMessage(error);
                })
            } else {
                this.writeErrorMessage("请输入查询参数");
            }
        } catch (e) {
            this.writeErrorMessage(e);
            console.error(e);
        } finally {
            this.hideLoading();
        }
    }

}).call(pwclog);