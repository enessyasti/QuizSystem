package Quiz.impl;

import Quiz.Interface.Answer;
import Quiz.Interface.Question;
import java.util.List;

public class BasicQuestion implements Question {
    private final String prompt;                // Soru metni
    private final List<Answer> answers;         // Cevap listesi
    private final int correctAnswerIndex;       // Doğru cevabın indeksi

    public BasicQuestion(String prompt, List<Answer> answers) {
        this.prompt = prompt;
        this.answers = List.copyOf(answers);                // listeyi dondur (kimse değiştiremesin)
        this.correctAnswerIndex = findCorrectIndex(answers); // otomatik doğru cevabı bul
    }

    // Cevaplar arasında dolaş, doğru olanın sırasını bul
    private int findCorrectIndex(List<Answer> list) {
        for (int i = 0; i < list.size(); i++) {
            if (list.get(i).isCorrect()) return i;
        }
        return -1; // hiç doğru yoksa
    }

    @Override public String getPrompt() { return prompt; }
    @Override public List<Answer> getAnswers() { return answers; }
    @Override public int getCorrectAnswerIndex() { return correctAnswerIndex; }
}