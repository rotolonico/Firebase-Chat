const functions = require('firebase-functions');

exports.profanityFilter = functions.database.ref('messages/{messageId}/text')
    .onWrite((snap, context) => {
        var text = snap.after.val();
        var updatedText = text.replace("Fortnite", "Minecraft");
        snap.after.ref.set(updatedText);
        return updatedText;
    });
