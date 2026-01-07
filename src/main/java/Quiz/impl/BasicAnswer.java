package Quiz.impl;

import Quiz.Interface.Answer;
import com.google.gson.annotations.SerializedName;

public class BasicAnswer implements Answer {
    // JSON dosyasındaki "text" alanını bu değişkene eşler
    @SerializedName("text")
    private final String content;
    private final boolean correct;

    public BasicAnswer(String content, boolean correct) {
        this.content = content;
        this.correct = correct;
    }

    // Interface gerekliliği: Metni döndür
    @Override
    public String getContent() {
        return content;
    }

    // Interface gerekliliği: Doğru mu?
    @Override
    public boolean isCorrect() {
        return correct;
    }
}