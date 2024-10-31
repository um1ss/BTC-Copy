const BalancyWebGL = {
    getTokens:  function (req_id) {
        req_id = UTF8ToString(req_id);
        if (!balancyData || !balancyData.user || !balancyData.user.oauthToken || !balancyData.user.oauthTokenSecret) {
            gameInstance.SendMessage('BalancyWebGLObject', 'OnTokens', JSON.stringify({
                req_id:   req_id,
                response: {
                    success: false,
                    data:    'no tokens info'
                }
            }));

            return;
        }

        gameInstance.SendMessage('BalancyWebGLObject', 'OnTokens', JSON.stringify({
            req_id:   req_id,
            response: {
                success: true,
                data:    JSON.stringify({
                    oauthToken:       balancyData.user.oauthToken,
                    oauthTokenSecret: balancyData.user.oauthTokenSecret,
                    id:               balancyData.user.id
                })
            }
        }));
    },
    getUserId:  function (req_id) {
        req_id = UTF8ToString(req_id);
        if (balancyData.user && req_id) {
            gameInstance.SendMessage('BalancyWebGLObject', 'OnUserId', JSON.stringify({
                req_id:   req_id,
                response: {
                    success: true,
                    data:    JSON.stringify({id: balancyData.user.id})
                }
            }));

            return;
        }

        const req = opensocial.newDataRequest();
        req.add(req.newFetchPersonRequest(opensocial.IdSpec.PersonId.VIEWER), 'viewer');
        req.send(function (response) {
            if (response.hadError()) {
                gameInstance.SendMessage('BalancyWebGLObject', 'OnUserId', JSON.stringify({
                    req_id:   req_id,
                    response: {
                        success: false,
                        data:    response.getErrorMessage()
                    }
                }));
            } else {
                const item = response.get('viewer');

                if (item.hadError()) {
                    gameInstance.SendMessage('BalancyWebGLObject', 'OnUserId', JSON.stringify({
                        req_id:   req_id,
                        response: {
                            success: false,
                            data:    item.getErrorMessage()
                        }
                    }));
                } else {
                    const viewer   = item.getData();
                    const id       = viewer.getId();
                    const nickname = viewer.getDisplayName();

                    balancyData.user = {
                        id:       id,
                        nickname: nickname
                    };

                    if (req_id)
                        gameInstance.SendMessage('BalancyWebGLObject', 'OnUserId', JSON.stringify({
                            req_id:   req_id,
                            response: {
                                success: true,
                                data:    JSON.stringify({id: balancyData.user.id})
                            }
                        }));
                }
            }
        });
    },
    purchaseSP: function (env, id, name, price, img, desc, req_id) {
        const itemId      = UTF8ToString(id);
        const itemName    = UTF8ToString(name);
        const imageUrl    = UTF8ToString(img);
        const description = UTF8ToString(desc);

        window.location = `https://nutaku-1.webhooks.balancy.dev/v1/nutaku/play/purchase/${itemId}?env=${env}&name=${itemName}&icon=${imageUrl}&description=${description}&${balancyData.query}`;
    },
    purchase:   function (env, id, name, price, img, desc, req_id) {
        const itemId      = UTF8ToString(id);
        const itemName    = UTF8ToString(name);
        const imageUrl    = UTF8ToString(img);
        const description = UTF8ToString(desc);
        req_id            = UTF8ToString(req_id);

        const itemParams                                     = {};
        itemParams[opensocial.BillingItem.Field.SKU_ID]      = itemId;
        itemParams[opensocial.BillingItem.Field.PRICE]       = price;
        itemParams[opensocial.BillingItem.Field.COUNT]       = 1;
        itemParams[opensocial.BillingItem.Field.DESCRIPTION] = description;
        itemParams[nutaku.BillingItem.Field.NAME]            = itemName;
        itemParams[nutaku.BillingItem.Field.IMAGE_URL]       = `${imageUrl}?environment=${env}`;

        const item = opensocial.newBillingItem(itemParams);

        const paymentParams                                  = {};
        paymentParams[opensocial.Payment.Field.ITEMS]        = [item];
        paymentParams[opensocial.Payment.Field.PAYMENT_TYPE] = opensocial.Payment.PaymentType.PAYMENT;

        const payment = opensocial.newPayment(paymentParams);

        opensocial.requestPayment(payment, function (response) {
            if (response.hadError()) {
                gameInstance.SendMessage('BalancyWebGLObject', 'OnPurchaseConfirmed', JSON.stringify({
                    req_id:   req_id,
                    response: {
                        success: false,
                        data:    response.getErrorMessage()
                    }
                }));
            } else {
                const payment      = response.getData();
                const paymentId    = payment.getField(nutaku.Payment.Field.PAYMENT_ID);
                const responseCode = payment.getField(opensocial.Payment.Field.RESPONSE_CODE);

                if (responseCode === opensocial.Payment.ResponseCode.OK)
                    gameInstance.SendMessage('BalancyWebGLObject', 'OnPurchaseConfirmed', JSON.stringify({
                        req_id:   req_id,
                        response: {
                            success: true,
                            data:    JSON.stringify({payment_id: paymentId})
                        }
                    }));
                else
                    gameInstance.SendMessage('BalancyWebGLObject', 'OnPurchaseConfirmed', JSON.stringify({
                        req_id:   req_id,
                        response: {
                            success: false,
                            data:    responseCode
                        }
                    }));
            }
        });
    },
    auth:       function (environment, game_id, device_id, token, req_id) {
        if (!environment)
            environment = 0;

        if (token) {
            token = UTF8ToString(token);
        }

        if (!game_id) {
            console.error('game_id not specified!');
            return;
        } else {
            game_id = UTF8ToString(game_id);
        }

        if (!device_id) {
            console.error('device_id not specified!');
            return;
        } else {
            device_id = UTF8ToString(device_id);
        }

        req_id = UTF8ToString(req_id);

        var params                                         = {};
        params[gadgets.io.RequestParameters.AUTHORIZATION] = gadgets.io.AuthorizationType.SIGNED;
        params[gadgets.io.RequestParameters.METHOD]        = gadgets.io.MethodType.POST;
        params[gadgets.io.RequestParameters.CONTENT_TYPE]  = gadgets.io.ContentType.JSON;

        const data = {
            env:       environment,
            game_id:   game_id,
            device_id: device_id
        };
        if (token) {
            data['token'] = token;
        }
        params[gadgets.io.RequestParameters.POST_DATA] = gadgets.io.encodeValues(data);

        gadgets.io.makeRequest(
            'https://api-1.api.balancy.dev/v2/auth/nutaku_pc',
            function (response) {
                if (response.errors.length > 0) {
                    gameInstance.SendMessage('BalancyWebGLObject', 'OnAuth', JSON.stringify({
                        req_id:   req_id,
                        response: {
                            success: false,
                            data:    JSON.stringify(response.errors[0])
                        }
                    }));
                } else {
                    const returnStr = JSON.stringify({
                        req_id:   req_id,
                        response: {
                            success: true,
                            data:    response.text
                        }
                    });
                    gameInstance.SendMessage('BalancyWebGLObject', 'OnAuth', returnStr);
                }
            },
            params);
    }
}


mergeInto(LibraryManager.library, BalancyWebGL);
